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
using Warshti.Entities.WScore;

namespace wscore.Controllers
{
    [Route("api/notifications")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class NotificationController : ControllerBase
    {
        private readonly WScoreContext _context;
        private readonly IMapper _mapper;

        public NotificationController(WScoreContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/notifications
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NotificationModel>>> Get()
        {
            var user = _context.Users.FirstOrDefault(x => x.Email == User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }

            var temp = await _context.Notifications
                .Include(p => p.User)
                .Where(p => p.UserId == user.Id)
                .ToListAsync();
            return Ok(_mapper.Map<IEnumerable<NotificationModel>>(temp));
        }

        // GET: api/notifications/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NotificationModel>> Get(int id)
        {
            var color = await _context.Notifications
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (color == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<NotificationModel>(color));
        }

        [HttpPost]
        public async Task<ActionResult<NotificationModel>> Create(NotificationCreateModel model)
        {
            var user = _context.Users.FirstOrDefault(x => x.Email == User.Identity.Name);
            if (user == null)
            {
                return NotFound();
            }
            
            var temp = new Notification
            {
                Message = model.Message,
                UserId = user.Id 
            };

            _context.Notifications.Add(temp);
            if (await _context.SaveChangesAsync() > 0)
            {
                return Created($"api/notifications/{temp.Id}",
                        _mapper.Map<NotificationModel>(temp));
            }
            return BadRequest("Error creating resource");
        }

        // DELETE: api/colors/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification == null)
            {
                return NotFound();
            }

            _context.Notifications.Remove(notification);
            if (await _context.SaveChangesAsync() > 0)
            {
                return NoContent();
            }

            return BadRequest("Error removing resource");
        }

        private bool Exists(int id)
        {
            return _context.Notifications.Any(e => e.Id == id);
        }
    }
}
