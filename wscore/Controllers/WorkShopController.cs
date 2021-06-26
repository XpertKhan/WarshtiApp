using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Warshti.Entities;
using WScore.Entities.Identity;
using WScore.Helpers;
using WScore.Models;

namespace WScore.Controllers
{
    [Route("/api/workshops")]
    [ApiController]
   // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class WorkShopController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly AccountHelper _accountHelper;
        private readonly DistanceHelper _distanceHelper;

        public WorkShopController(UserManager<User> userManager,
            IMapper mapper,
            AccountHelper accountHelper,
            DistanceHelper distanceHelper)
        {
            _userManager = userManager;
            _mapper = mapper;
            _accountHelper = accountHelper;
            _distanceHelper = distanceHelper;
        }

        [HttpPost()]
        public ActionResult Get([FromBody] UserLocation location)
        {
            var users = _userManager.Users
                .Include(p => p.WorkShopInfo)
                .Include(p => p.WorkShopImages)
                .Where(p => p.UserTypeId == (int)UserType.Workshop);
            var result = _mapper.Map<IEnumerable<WorkShopUserModel>>(users);
            foreach (var workshop in result)
            {
                double distance = 0;

                if (workshop.WorkShopInfo != null && location.Latitude.HasValue && location.Longitude.HasValue)
                {
                    distance = _distanceHelper.Distance(
                        location.Latitude.ToString(),
                        location.Longitude.ToString(),
                        workshop.WorkShopInfo.Latitude.ToString(),
                        workshop.WorkShopInfo.Longitude.ToString());
                }
                workshop.Distance = distance;
            }
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var user = await _userManager.Users
                .Include(p => p.WorkShopInfo)
                .Include(p => p.WorkShopImages)
                .FirstOrDefaultAsync(p => p.Id == id && p.UserTypeId == (int)UserType.Workshop);
            if (user == null)
            {
                return NotFound("Not a workshop");
            }

            return Ok(_mapper.Map<WorkShopUserModel>(user));
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
                    UserTypeId = (int)UserType.Workshop,
                    VerificationToken = AccountHelper.RandomTokenString()
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _accountHelper.SendVerificationEmail(user, Request.Headers["origin"]);
                    return Created($"/api/workshops/{user.Id}", _mapper.Map<WorkShopUserModel>(user));
                }
                return BadRequest("Error creating user");
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
        }
        #endregion
    }
}
