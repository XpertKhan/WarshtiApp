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
using Warshti.Entities.Maintenance;

namespace wscore.Controllers
{
    [Route("api/departments")]
    [ApiController]
   // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class DepartmentController : ControllerBase
    {
        private readonly WScoreContext _context;
        private readonly IMapper _mapper;

        public DepartmentController(WScoreContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Departments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DepartmentModel>>> Get()
        {
            var temp = await _context.Departments.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<DepartmentModel>>(temp));
        }

        // GET: api/Departments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DepartmentModel>> Get(int id)
        {
            var Department = await _context.Departments.FindAsync(id);

            if (Department == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<DepartmentModel>(Department));
        }

        // PUT: api/Departments/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, DepartmentUpdateModel Department)
        {
            var temp = await _context.Departments.FirstOrDefaultAsync(p => p.Id == id);
            if (temp == null)
            {
                return BadRequest();
            }


            temp.Name = Department.Name;

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

        // POST: api/Departments
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<DepartmentModel>> Create(DepartmentCreateModel Department)
        {
            var temp = new Department
            {
                Name = Department.Name
            };

            _context.Departments.Add(temp);
            if (await _context.SaveChangesAsync() > 0)
            {
                return Created($"api/Departments/{temp.Id}",
                        _mapper.Map<DepartmentModel>(temp));
            }
            return BadRequest("Error creating resource");
        }

        // DELETE: api/Departments/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var Departments = await _context.Departments.FindAsync(id);
            if (Departments == null)
            {
                return NotFound();
            }

            _context.Departments.Remove(Departments);
            if (await _context.SaveChangesAsync() > 0)
            {
                return NoContent();
            }

            return BadRequest("Error removing resource");
        }

        private bool Exists(int id)
        {
            return _context.Departments.Any(e => e.Id == id);
        }
    }
}
