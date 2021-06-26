using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using WScore.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Warshti.Entities;
using Warshti.Entities.Identity;
using System.Security.Claims;
using WScore;

namespace wscore.Controllers
{
    [Route("api/usersettings")]
    [ApiController]
  //  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class UserSettingController : ControllerBase
    {
        private readonly WScoreContext _context;
        private readonly IMapper _mapper;

        public UserSettingController(WScoreContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet()]
        public async Task<ActionResult<UserSettingModel>> Get()
        {
            var account = _context.Users.SingleOrDefault(x => x.PhoneNumber == User.Identity.Name);

            if (account == null) throw new Exception("Verification failed");

            var temp = await _context.UserSettings
                .Include(p => p.User)
                .Include(p => p.Language)
                .FirstOrDefaultAsync(p => p.UserId == account.Id);

            if (temp == null)
            {
              return Ok(Constants.NoContentFound);
            }


            return Ok(_mapper.Map<UserSettingModel>(temp));
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update( [FromBody]UserSettingUpdateModel model)
        {
            int userId = User.Claims.Where(e => e.Type == ClaimTypes.NameIdentifier).Select(e => int.Parse(e.Value)).FirstOrDefault();
            var account = await _context.Users
                .Include(p => p.UserSetting)
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (account == null) throw new Exception("Verification failed");

            var temp = await _context.UserSettings
                .FirstOrDefaultAsync(p => p.UserId == account.Id);
            if (temp == null)
            {
                return BadRequest();
            }


            temp.EmailNotification = model.EmailNotification;
            temp.AcceptRequest = model.AcceptRequest;
            temp.DeclineRequest = model.DeclineRequest;
            temp.LanguageId = model.LanguageId;

            _context.Entry(temp).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Exists(temp.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost("create")]
        public async Task<ActionResult<UserSettingModel>> Create([FromBody] UserSettingCreateModel model)
        {
            int userId = User.Claims.Where(e => e.Type == ClaimTypes.NameIdentifier).Select(e=>int.Parse(e.Value)).FirstOrDefault();
            var account = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);

            if (account == null) throw new Exception("Verification failed");

            var setting = await _context.UserSettings
                .FirstOrDefaultAsync(p => p.UserId == account.Id);
            if (setting != null)
            {
                return BadRequest("User settings already exixts. You may update.");
            }

            var temp = new UserSetting
            {
                EmailNotification = model.EmailNotification,
                AcceptRequest = model.AcceptRequest,
                DeclineRequest = model.DeclineRequest,
                LanguageId = model.LanguageId,
                UserId = account.Id
            };

            _context.UserSettings.Add(temp);
            if (await _context.SaveChangesAsync() > 0)
            {
                return Created($"api/usersettings/{temp.Id}",
                        _mapper.Map<UserSettingModel>(temp));
            }
            return BadRequest("Error creating resource");
        }

        private bool Exists(int id)
        {
            return _context.UserSettings.Any(e => e.Id == id);
        }
    }
}
