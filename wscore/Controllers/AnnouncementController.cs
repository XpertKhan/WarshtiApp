using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Warshti.Entities;
using Warshti.Entities.WScore;
using WScore.Models;

namespace WScore.Controllers
{
    [Route("/api/announcements")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class AnnouncementController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly WScoreContext _context;

        public AnnouncementController(WScoreContext context,
            IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        #region Read

        [HttpGet()]
        public ActionResult Get()
        {
            var model = _context.Announcements
                            .Include(p => p.AnnouncementImages);
            return Ok(_mapper.Map<IEnumerable<AnnouncementModel>>(model));
        }

        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            var model = _context.Announcements
                            .Include(p => p.AnnouncementImages)
                            .FirstOrDefault(p => p.Id == id);
            if (model == null)
            {
                return NotFound("Not an announcement");
            }

            return Ok(_mapper.Map<AnnouncementModel>(model));
        }
        #endregion

        #region Create
        [HttpPost()]
        public ActionResult Create([FromForm] AnnouncementCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var temp = new Announcement
                {
                    Description = model.Description,
                    Detail = model.Detail,
                    Title = model.Title,
                    UserId = model.UserId
                };

                List<AnnouncementImage> images = new List<AnnouncementImage>();
                foreach (var item in model.Photos)
                {
                    using (var target = new MemoryStream())
                    {
                        item.CopyTo(target);
                        images.Add(new AnnouncementImage
                        {
                            Photo = target.ToArray()
                        });
                    }
                }

                temp.AnnouncementImages = images;

                _context.Announcements.Add(temp);
                if (_context.SaveChanges() > 0)
                {
                    return Created($"api/announcements/{temp.Id}",
                        _mapper.Map<AnnouncementModel>(temp));
                }
            }

            return BadRequest(ModelState);
        }
        #endregion

        #region Update
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromForm] AnnouncementUpdateModel model)
        {
            if (ModelState.IsValid)
            {
                var temp = await _context.Announcements
                    .Include(p=>p.AnnouncementImages)
                    .FirstOrDefaultAsync(p => p.Id == id);
                if (temp == null)
                {
                    return NotFound("Announcement Not found");
                }

                foreach (var image in temp.AnnouncementImages)
                {
                    _context.AnnouncementImages.Remove(image);
                }

                List<AnnouncementImage> images = new List<AnnouncementImage>();
                foreach (var item in model.Photos)
                {
                    using (var target = new MemoryStream())
                    {
                        item.CopyTo(target);
                        images.Add(new AnnouncementImage
                        {
                            Photo = target.ToArray()
                        });
                    }
                }

                temp.Description = model.Description;
                temp.Detail = model.Detail;
                temp.Title = model.Title;
                temp.AnnouncementImages = images;

                _context.Announcements.Update(temp);
                if (_context.SaveChanges() > 0)
                {
                    return NoContent();
                }
            }

            return BadRequest(ModelState);
        }
        #endregion

        #region Delete
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var announcement = await _context.Announcements
                .Include(p => p.AnnouncementImages)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (announcement == null)
            {
                return NotFound();
            }

            foreach (var image in announcement.AnnouncementImages)
            {
                _context.Remove(image);
            }

            _context.Announcements.Remove(announcement);
            if (await _context.SaveChangesAsync() > 0)
            {
                return NoContent();
            }

            return BadRequest("Error removing resource");
        }
        #endregion
    }
}
