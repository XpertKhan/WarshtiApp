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
    [Route("api/models")]
    [ApiController]
  //  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class ModelController : ControllerBase
    {
        private readonly WScoreContext _context;
        private readonly IMapper _mapper;

        public ModelController(WScoreContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Models
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ModelModel>>> Get()
        {
            var temp = await _context.Models.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<ModelModel>>(temp));
        }

        // GET: api/Models/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ModelModel>> Get(int id)
        {
            var Model = await _context.Models.FindAsync(id);

            if (Model == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ModelModel>(Model));
        }

        // PUT: api/Models/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ModelUpdateModel Model)
        {
            var temp = await _context.Models.FirstOrDefaultAsync(p => p.Id == id);
            if (temp == null)
            {
                return BadRequest();
            }


            temp.Name = Model.Name;

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

        // POST: api/Models
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ModelModel>> Create(ModelCreateModel Model)
        {
            var temp = new Model
            {
                Name = Model.Name
            };

            _context.Models.Add(temp);
            if (await _context.SaveChangesAsync() > 0)
            {
                return Created($"api/models/{temp.Id}",
                        _mapper.Map<ModelModel>(temp));
            }
            return BadRequest("Error creating resource");
        }

        // DELETE: api/Models/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var Models = await _context.Models.FindAsync(id);
            if (Models == null)
            {
                return NotFound();
            }

            _context.Models.Remove(Models);
            if (await _context.SaveChangesAsync() > 0)
            {
                return NoContent();
            }

            return BadRequest("Error removing resource");
        }

        private bool Exists(int id)
        {
            return _context.Models.Any(e => e.Id == id);
        }
    }
}
