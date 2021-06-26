using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Warshti.Panel.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Warshti.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using WScore.Entities.Identity;

namespace Warshti.Panel.Controllers
{
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public AuthController(ILogger<AuthController> logger,
            SignInManager<User> signInManager,
            UserManager<User> userManager)
        {
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        #region Login
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var result = await _signInManager.PasswordSignInAsync(
                        userName: model.Username,
                        password: model.Password,
                        isPersistent: model.RememberMe,
                        lockoutOnFailure: false);

                    if (result.Succeeded)
                    {
                        var user = await _userManager.Users.FirstOrDefaultAsync(p => p.UserName == model.Username && p.IsActive == true);
                        if (user == null)
                        {
                            await _signInManager.SignOutAsync();
                            return RedirectToAction("Login", new { area = "", controller = "Auth" });
                        }

                        if (!String.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                            return Redirect(returnUrl);
                        else
                            return RedirectToAction("Index", new { area = "", controller = "Home" });
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                ModelState.AddModelError("", "Error has occured");
            }


            return View(model);
        }
        #endregion

        #region SignUp
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SignUp(SignUpViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return RedirectToAction("Login", new { controller = "Auth" });
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                ModelState.AddModelError("", "Error has occured");
            }

            return View(model);
        }
        #endregion

        #region Forget Password
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ForgetPassword(ForgetPasswordViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return RedirectToAction("Login", new { controller = "Auth" });
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                ModelState.AddModelError("", "Error has occured");
            }

            return View(model);
        }
        #endregion

        #region SignOut
        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", new { controller = "Auth" });
        }
        #endregion

        #region Change Password
        [Authorize]
        [HttpGet()]
        public async Task<IActionResult> ChangePassword()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }

            var model = new ChangePasswordViewModel()
            {
                UserId = user.Id
            };

            return PartialView(model);
        }

        [Authorize]
        [HttpPost()]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(p => p.Id == model.UserId);
            if (user == null)
            {
                return NotFound();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("", "Error while saving data");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Provide all required data to proceed");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                ModelState.AddModelError("", "Error has occured");
            }

            return PartialView(model);
        }
        #endregion
    }
}
