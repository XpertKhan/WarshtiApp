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
    [Route("api/faults")]
    [ApiController]
 //   [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class FaultController : ControllerBase
    {
        private readonly WScoreContext _context;
        private readonly IMapper _mapper;

        public FaultController(WScoreContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Faults
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FaultModel>>> Get()
        {
            var temp = await _context.Faults.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<FaultModel>>(temp));
        }

        // GET: api/Faults/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FaultModel>> Get(int id)
        {
            var Fault = await _context.Faults.FindAsync(id);

            if (Fault == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<FaultModel>(Fault));
        }

        // PUT: api/Faults/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, FaultUpdateModel Fault)
        {
            var temp = await _context.Faults.FirstOrDefaultAsync(p => p.Id == id);
            if (temp == null)
            {
                return BadRequest();
            }


            temp.Name = Fault.Name;

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

        // POST: api/Faults
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<FaultModel>> Create(FaultCreateModel Fault)
        {
            var temp = new Fault
            {
                Name = Fault.Name
            };

            _context.Faults.Add(temp);
            if (await _context.SaveChangesAsync() > 0)
            {
                return Created($"api/Faults/{temp.Id}",
                        _mapper.Map<FaultModel>(temp));
            }
            return BadRequest("Error creating resource");
        }

        // DELETE: api/Faults/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var Faults = await _context.Faults.FindAsync(id);
            if (Faults == null)
            {
                return NotFound();
            }

            _context.Faults.Remove(Faults);
            if (await _context.SaveChangesAsync() > 0)
            {
                return NoContent();
            }

            return BadRequest("Error removing resource");
        }

        private bool Exists(int id)
        {
            return _context.Faults.Any(e => e.Id == id);
        }
    }
}
