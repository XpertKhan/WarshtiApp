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
    [Route("/api/client")]
    [ApiController]
   // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class ClientController : Controller
    {
        private readonly WScoreContext _context;
        private readonly AccountHelper _accountHelper;
        private readonly UserManager<User> _userManager;

        public ClientController(WScoreContext context, AccountHelper accountHelper, UserManager<User> userManager)
        {
            this._context = context;

            _accountHelper = accountHelper;
            this._userManager = userManager;
        }
        [HttpGet("{user_id}")]
        // GET: ClientController
        public async Task<User> Get([FromRoute] int user_id)
        {
            var user = await _context.Users.Where(e => e.Id == user_id).FirstOrDefaultAsync();
            return user;
        }

        [HttpPut("updateuser/{user_id}")]
        public async Task<IActionResult> UpdateUser(int user_id, UpdateUser userModel, string password = "")
        {
            var user = _context.Users.Where(e => e.Id == user_id).FirstOrDefault();
            user.Name = userModel.Name;
            user.PhoneNumber = userModel.PhoneNumber;
            user.Email = userModel.Email;
            _context.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await _context.SaveChangesAsync();
            if (!String.IsNullOrEmpty(password))
            {
                await _userManager.RemovePasswordAsync(user);
                await _userManager.AddPasswordAsync(user, password);
            }
            return Ok();
        }


        
    }
}
