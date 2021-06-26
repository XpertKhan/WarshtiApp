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
using Warshti.Entities.Car;

namespace wscore.Controllers
{
    [Route("api/colors")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class ColorController : ControllerBase
    {
        private readonly WScoreContext _context;
        private readonly IMapper _mapper;

        public ColorController(WScoreContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/colors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ColorModel>>> Get()
        {
            var temp = await _context.Colors.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<ColorModel>>(temp));
        }

        // GET: api/colors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ColorModel>> Get(int id)
        {
            var color = await _context.Colors.FindAsync(id);

            if (color == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ColorModel>(color));
        }

        // PUT: api/colors/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ColorUpdateModel color)
        {
            var temp = await _context.Colors.FirstOrDefaultAsync(p => p.Id == id);
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
        public async Task<ActionResult<ColorModel>> Create(ColorCreateModel color)
        {
            var temp = new Color
            {
                Name = color.Name
            };

            _context.Colors.Add(temp);
            if (await _context.SaveChangesAsync() > 0)
            {
                return Created($"api/colors/{temp.Id}",
                        _mapper.Map<ColorModel>(temp));
            }
            return BadRequest("Error creating resource");
        }

        // DELETE: api/colors/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var colors = await _context.Colors.FindAsync(id);
            if (colors == null)
            {
                return NotFound();
            }

            _context.Colors.Remove(colors);
            if (await _context.SaveChangesAsync() > 0)
            {
                return NoContent();
            }

            return BadRequest("Error removing resource");
        }

        private bool Exists(int id)
        {
            return _context.Colors.Any(e => e.Id == id);
        }
    }
}
