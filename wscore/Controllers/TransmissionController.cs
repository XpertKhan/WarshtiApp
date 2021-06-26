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
    [Route("api/transmissions")]
    [ApiController]
  //  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class TransmissionController : ControllerBase
    {
        private readonly WScoreContext _context;
        private readonly IMapper _mapper;

        public TransmissionController(WScoreContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Transmissions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransmissionModel>>> Get()
        {
            var temp = await _context.Transmissions.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<TransmissionModel>>(temp));
        }

        // GET: api/Transmissions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TransmissionModel>> Get(int id)
        {
            var Transmission = await _context.Transmissions.FindAsync(id);

            if (Transmission == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<TransmissionModel>(Transmission));
        }

        // PUT: api/Transmissions/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TransmissionUpdateModel Transmission)
        {
            var temp = await _context.Transmissions.FirstOrDefaultAsync(p => p.Id == id);
            if (temp == null)
            {
                return BadRequest();
            }


            temp.Name = Transmission.Name;

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

        // POST: api/Transmissions
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<TransmissionModel>> Create(TransmissionCreateModel Transmission)
        {
            var temp = new Transmission
            {
                Name = Transmission.Name
            };

            _context.Transmissions.Add(temp);
            if (await _context.SaveChangesAsync() > 0)
            {
                return Created($"api/transmissions/{temp.Id}",
                        _mapper.Map<TransmissionModel>(temp));
            }
            return BadRequest("Error creating resource");
        }

        // DELETE: api/Transmissions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var Transmissions = await _context.Transmissions.FindAsync(id);
            if (Transmissions == null)
            {
                return NotFound();
            }

            _context.Transmissions.Remove(Transmissions);
            if (await _context.SaveChangesAsync() > 0)
            {
                return NoContent();
            }

            return BadRequest("Error removing resource");
        }

        private bool Exists(int id)
        {
            return _context.Transmissions.Any(e => e.Id == id);
        }
    }
}
