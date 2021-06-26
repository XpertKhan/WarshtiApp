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
    [Route("api/languages")]
    [ApiController]
   // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class LanguageController : ControllerBase
    {
        private readonly WScoreContext _context;
        private readonly IMapper _mapper;

        public LanguageController(WScoreContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/colors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LanguageModel>>> Get()
        {
            var temp = await _context.Languages.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<LanguageModel>>(temp));
        }

        // GET: api/colors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LanguageModel>> Get(int id)
        {
            var color = await _context.Languages.FindAsync(id);

            if (color == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<LanguageModel>(color));
        }

        // PUT: api/colors/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, LanguageUpdateModel color)
        {
            var temp = await _context.Languages.FirstOrDefaultAsync(p => p.Id == id);
            if (temp == null)
            {
                return BadRequest();
            }


            temp.Name = color.Name;

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
        public async Task<ActionResult<LanguageModel>> Create(LanguageCreateModel color)
        {
            var temp = new Language
            {
                Name = color.Name
            };

            _context.Languages.Add(temp);
            if (await _context.SaveChangesAsync() > 0)
            {
                return Created($"api/languages/{temp.Id}",
                        _mapper.Map<LanguageModel>(temp));
            }
            return BadRequest("Error creating resource");
        }

        // DELETE: api/colors/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var colors = await _context.Languages.FindAsync(id);
            if (colors == null)
            {
                return NotFound();
            }

            _context.Languages.Remove(colors);
            if (await _context.SaveChangesAsync() > 0)
            {
                return NoContent();
            }

            return BadRequest("Error removing resource");
        }

        private bool Exists(int id)
        {
            return _context.Languages.Any(e => e.Id == id);
        }
    }
}
