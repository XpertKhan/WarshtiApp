using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Warshti.Entities;
using Warshti.Entities.Entities;
using WScore.Entities.Identity;
using WScore.Helpers;
using WScore.Models;

namespace WScore.Controllers
{
    [Route("/api/accounts")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _config;
        private readonly WScoreContext _context;
        private readonly IEmailService _emailService;
        private readonly AccountHelper _accountHelper;

        public AccountController(UserManager<User> userManager,
            AccountHelper accountHelper,
            SignInManager<User> signInManager,
            IMapper mapper,
            IConfiguration config,
            IEmailService emailService,
            WScoreContext context)
        {
            _userManager = userManager;
            _mapper = mapper;
            _signInManager = signInManager;
            _config = config;
            _context = context;
            _emailService = emailService;
            _accountHelper = accountHelper;
        }


        #region Register
        /// <summary>
        /// his Register method can only be used by mealmate admin portal for registering new Reataurant Owners 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>A newly created User</returns>
        /// <response code="201">Returns the newly created User</response>
        /// <response code="400">If the item is null</response>      
        [AllowAnonymous]

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Register([FromBody] RegisterModel model)
        {
            try
            {
                var user = new User
                {
                    UserName = model.PhoneNumber,
                    Name = model.Name,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    UserTypeId = (int)model.Type,
                    VerificationToken = AccountHelper.RandomTokenString(),
                    IsActive = true
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    user.IsActive = true;
                    _context.Entry(user).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    await _accountHelper.SendVerificationEmail(user, Request.Headers["origin"]);
                    return Created($"/api/accounts/{user.Id}", _mapper.Map<UserModel>(user));
                }
                return Ok("User created successfully");
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
        }
        #endregion

        #region Create JWT
        private async Task<AuthenticationModel> GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("id", user.Id.ToString())
            };

            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config["Tokens:Key"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = creds,
                Issuer = _config["Tokens:Issuer"],
                Audience = _config["Tokens:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var refreshToken = new RefreshToken
            {
                JwtId = token.Id,
                UserId = user.Id,
                CreationDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(6)
            };

            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            return new AuthenticationModel
            {
                Success = true,
                Token = tokenHandler.WriteToken(token),
                RefreshToken = refreshToken.Id
            };

        }
        private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
        {
            return (validatedToken is JwtSecurityToken jwtSecurityToken) &&
                   jwtSecurityToken.Header.Alg.Equals(
                       SecurityAlgorithms.HmacSha512,
                       StringComparison.InvariantCultureIgnoreCase);
        }
        #endregion

        #region Login
        [AllowAnonymous]
        [HttpPost("signin")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SignIn([FromBody] LoginModel request)
        {
            try
            {
                var user = _context.Users.Where(e => e.UserName == request.Mobile).FirstOrDefault();

                if (user != null)
                {
                    var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

                    
                    if (result.Succeeded)
                    {
                        var appUser = await _userManager.Users
                                                        .FirstOrDefaultAsync(
                                                        u => u.UserName == request.Mobile.ToUpper());

                        if (!appUser.EmailConfirmed)
                        {
                            return Unauthorized(new
                            {
                                message = "Account email is not verified"
                            });
                        }
                        if (!appUser.IsActive)
                        {
                            return Unauthorized(new
                            {
                                message = "Account not active"
                            });
                        }
                        var userToReturn = _mapper.Map<UserModel>(appUser);
                        var authResponse = await GenerateJwtToken(appUser);
                        //Store token in database
                        //Find out if token already exists
                        var hasSameToken = _context.DeviceTokens.Where(e=>e.User_Id == user.Id && e.Device_Token == request.DeviceToken).Any();
                        if (!hasSameToken)
                        {
                            //Store in DB
                            var deviceToken = new DeviceToken();
                            deviceToken.Device_Token = request.DeviceToken;
                            deviceToken.User_Id = appUser.Id;
                            _context.Entry(deviceToken).State = EntityState.Added;
                            _context.SaveChanges();
                        }
                        return Ok(new
                        {
                            token = authResponse.Token,
                            refreshToken = authResponse.RefreshToken,
                            user = userToReturn,
                        });
                    }
                    else
                    {
                        return Unauthorized("Incorrect username / password");
                    }
                }
                return Unauthorized("Incorrect username / password");
            }
            catch (Exception)
            {
                return Unauthorized("Error while processing request");
            }
        }
        #endregion

        [Authorize]
        [Route("deactivate-account")]
        [HttpPost]
        public async Task<IActionResult> DeactivateAccount([FromQuery] int userId)
        {
            var account = _context.Users.Where(e => e.Id == userId).FirstOrDefault();
            account.IsActive = false;
            _context.Entry(account).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok();
        } 

        #region Forgot Password OTP by Email
        /// <summary>
        /// Generates and OPT to Reset the password and sends to the user register mobile number
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// 
        [AllowAnonymous]
        [HttpPost()]
        [Route("forgot-password-email")]
        public async Task<ActionResult> ForgotPassword([FromBody] ForgotPasswordEmailModel model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    return NotFound($"User with email {model.Email} no more exists");
                }

                user.ResetToken = GetUniqueToken();
                user.ResetTokenExpires = DateTime.UtcNow.AddDays(1);

                _context.Users.Update(user);
                if (await _context.SaveChangesAsync() > 0)
                {
                    if (!string.IsNullOrEmpty(user.Email))
                    {
                        var message = $"Your OTP is {user.ResetToken}";
                        await _emailService.SendEmailAsync(model.Email, "Warshti OTP", message);
                        return Ok(model.Email);
                    }

                }
                return BadRequest("Error While Generating OTP");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [NonAction]
        public string GetUniqueToken() 
        {
            var _token = GenerateToken();
            do {
               _token= GenerateToken();
             }
            while (ValidateOTP(_token.ToString()));
            return _token.ToString();
        }
        [NonAction]
        public int GenerateToken()
        {
            return new Random().Next(100000, 999999);
        }
        [NonAction]
        public bool ValidateOTP(string otpToken)
        {
            return _context.Users.Any(x =>
              x.ResetToken == otpToken &&
              x.ResetTokenExpires > DateTime.UtcNow);
           
        }
        #endregion

        #region Forgot Password OTP by Mobile
        [AllowAnonymous]
        [HttpPost()]
        [Route("forgot-password-mobile")]
        public async Task<ActionResult> ForgotPassword([FromBody] ForgotPasswordMobileModel model)
        {
            try
            {
                var user = await _userManager.Users.FirstOrDefaultAsync(p => p.PhoneNumber == model.Mobile);
                if (user == null)
                {
                    return NotFound($"User with mobile {model.Mobile} no more exists");
                }

                user.ResetToken = GetUniqueToken();
                user.ResetTokenExpires = DateTime.UtcNow.AddDays(1);
                _context.Users.Update(user);
                if (await _context.SaveChangesAsync() > 0)
                {
                    if (!string.IsNullOrEmpty(user.PhoneNumber))
                    {
                        var accountId = _config["Twilio:AccountId"];
                        var token = _config["Twilio:Token"];
                        TwilioClient.Init(accountId, token);
                        var message = MessageResource.Create(
                            body: $"Your OTP is {user.ResetToken}",
                            from: new Twilio.Types.PhoneNumber(_config["Twilio:PhoneNumber"]),
                            to: new Twilio.Types.PhoneNumber(user.PhoneNumber)
                        );
                        return Ok(message.Sid);
                    }
                }
                return BadRequest("Error While Generating OTP");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Verify OTP
        [HttpPost("validate-reset-token")]
        public IActionResult ValidateResetToken(ValidateResetTokenModel model)
        {
            GetValidateResetToken(model);
            return Ok(new { message = "Token is valid" });
        }

        private void GetValidateResetToken(ValidateResetTokenModel model)
        {
            var account = _context.Users.SingleOrDefault(x =>
                x.ResetToken == model.Token &&
                x.ResetTokenExpires > DateTime.UtcNow);

            if (account == null)
                throw new Exception("Invalid token");
        }

        #endregion

        #region Verify Email
        [AllowAnonymous]
        [HttpGet("verify-email")]
        public IActionResult VerifyEmail(string token) 
        {
            var account = _context.Users.SingleOrDefault(x => x.VerificationToken == token);

            if (account == null) NotFound();

            account.EmailConfirmed = true;
            account.VerificationToken = string.Empty;

            _context.Users.Update(account);
            _context.SaveChanges();
            return Ok(new { message = "Verification successful, you can now login" });
        }
        #endregion

        #region Reset Password
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
           var _response= await GetResetPassword(model);
            if(_response=="InvalidToken")
                return Ok(new { message = "OTP token is not valid or expired" });
            return Ok(new { message = "Password reset successful, you can now login" });
        }

        private async Task<string> GetResetPassword(ResetPasswordModel model)
        {
            var account = _context.Users.SingleOrDefault(x =>
                x.ResetToken == model.Token &&
                x.ResetTokenExpires > DateTime.UtcNow);

            if (account == null)
                return "InvalidToken";

            // update password and remove reset token
            await _userManager.RemovePasswordAsync(account);
            await _userManager.AddPasswordAsync(account, model.Password);

            account.ResetToken = null;
            account.ResetTokenExpires = null;

            _context.Users.Update(account);
            _context.SaveChanges();
            return "ValidToken";
        }
        #endregion

        
    }
}
