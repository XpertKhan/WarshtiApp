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
    [Route("api/faqs")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class FaqController : ControllerBase
    {
        private readonly WScoreContext _context;
        private readonly IMapper _mapper;

        public FaqController(WScoreContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/fqas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FaqModel>>> Get()
        {
            var temp = await _context.Faqs.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<FaqModel>>(temp));
        }

        // GET: api/faqs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FaqModel>> Get(int id)
        {
            var color = await _context.Faqs.FindAsync(id);

            if (color == null)
            {
                return NotFound("No Faq found");
            }

            return Ok(_mapper.Map<FaqModel>(color));
        }

        // PUT: api/colors/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] FaqUpdateModel color)
        {
            var temp = await _context.Faqs.FirstOrDefaultAsync(p => p.Id == id);
            if (temp == null)
            {
                return NotFound("No Faq found");
            }


            temp.Question = color.Question;
            temp.Answer = color.Answer;

            _context.Entry(temp).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Exists(id))
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

        // POST: api/colors
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<FaqModel>> Create([FromBody]FaqCreateModel color)
        {
            var temp = new Faq
            {
                Question = color.Question,
                Answer = color.Answer
            };

            _context.Faqs.Add(temp);
            if (await _context.SaveChangesAsync() > 0)
            {
                return Created($"api/faqs/{temp.Id}",
                        _mapper.Map<FaqModel>(temp));
            }
            return BadRequest("Error creating resource");
        }

        // DELETE: api/colors/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var colors = await _context.Faqs.FindAsync(id);
            if (colors == null)
            {
                return NotFound();
            }

            _context.Faqs.Remove(colors);
            if (await _context.SaveChangesAsync() > 0)
            {
                return NoContent();
            }

            return BadRequest("Error removing resource");
        }

        private bool Exists(int id)
        {
            return _context.Faqs.Any(e => e.Id == id);
        }
    }
}
