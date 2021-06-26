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
    [Route("/api/questions")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class QuestionsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly WScoreContext _context;

        public QuestionsController(WScoreContext context,
            IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        #region Read

        [HttpGet()]
        public ActionResult Get()
        {
            var model = _context.Questions
                            .Include(p => p.QuestionImages)
                            .Include(p => p.User)
                            .OrderByDescending(e => e.Id).Take(5);
            return Ok(_mapper.Map<IEnumerable<QuestionModel>>(model));
        }

        [HttpGet("all")]
        public ActionResult All()
        {
            var model = _context.Questions
                            .Include(p => p.QuestionImages)
                            .Include(p => p.User)
                            .OrderByDescending(e => e.Id);
            return Ok(_mapper.Map<IEnumerable<QuestionModel>>(model));
        }

        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            var model = _context.Questions
                            .Include(p => p.QuestionImages)
                            .Include(p => p.Answers)
                            .ThenInclude(p => p.User)
                            .FirstOrDefault(p => p.Id == id);
            if (model == null)
            {
                return NotFound("Not a question");
            }

            return Ok(_mapper.Map<QuestionModel>(model));
        }
        #endregion

        #region Create
        [HttpPost()]
        public ActionResult Create([FromForm] QuestionCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var temp = new Question
                {
                    Description = model.Description,
                    Detail = model.Detail,
                    Title = model.Title,
                    UserId = model.UserId
                };

                List<QuestionImage> images = new List<QuestionImage>();
                foreach (var item in model.Photos)
                {
                    using (var target = new MemoryStream())
                    {
                        item.CopyTo(target);
                        images.Add(new QuestionImage
                        {
                            Photo = target.ToArray()
                        });
                    }
                }

                temp.QuestionImages = images;

                _context.Questions.Add(temp);
                if (_context.SaveChanges() > 0)
                {
                    return Created($"api/questions/{temp.Id}",
                        _mapper.Map<QuestionModel>(temp));
                }
            }

            return BadRequest(ModelState);
        }
        #endregion

        #region Update
        /*
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
        */
        #endregion

        #region Delete
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var announcement = await _context.Questions
                .Include(p => p.QuestionImages)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (announcement == null)
            {
                return NotFound();
            }

            foreach (var image in announcement.QuestionImages)
            {
                _context.Remove(image);
            }

            _context.Questions.Remove(announcement);
            if (await _context.SaveChangesAsync() > 0)
            {
                return NoContent();
            }

            return BadRequest("Error removing resource");
        }
        #endregion
    }
}
