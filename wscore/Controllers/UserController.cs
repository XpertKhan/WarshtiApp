using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Warshti.Entities;
using WScore.Entities.Identity;
using WScore.Models;

namespace WScore.Controllers
{
    [Route("/api/users")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public UserController(UserManager<User> userManager,
            IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpGet()]
        public ActionResult Get()
        {
            var users = _userManager.Users
                                    .Where(p => p.UserTypeId == (int)UserType.Client);
            return Ok(_mapper.Map<IEnumerable<UserModel>>(users));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(p => p.Id == id && p.UserTypeId == (int)UserType.Client);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<UserModel>(user));
        }
    }
}
