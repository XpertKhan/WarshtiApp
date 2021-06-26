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
    [Route("api/companies")]
    [ApiController]
  //  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class CompanyController : ControllerBase
    {
        private readonly WScoreContext _context;
        private readonly IMapper _mapper;

        public CompanyController(WScoreContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Companys
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompanyModel>>> Get()
        {
            var temp = await _context.Companies.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<CompanyModel>>(temp));
        }

        // GET: api/Companys/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CompanyModel>> Get(int id)
        {
            var Company = await _context.Companies.FindAsync(id);

            if (Company == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<CompanyModel>(Company));
        }

        // PUT: api/Companys/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CompanyUpdateModel Company)
        {
            var temp = await _context.Companies.FirstOrDefaultAsync(p => p.Id == id);
            if (temp == null)
            {
                return BadRequest();
            }


            temp.Name = Company.Name;

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

        // POST: api/Companys
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<CompanyModel>> Create(CompanyCreateModel Company)
        {
            var temp = new Company
            {
                Name = Company.Name
            };

            _context.Companies.Add(temp);
            if (await _context.SaveChangesAsync() > 0)
            {
                return Created($"api/Companys/{temp.Id}",
                        _mapper.Map<CompanyModel>(temp));
            }
            return BadRequest("Error creating resource");
        }

        // DELETE: api/Companys/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var Companys = await _context.Companies.FindAsync(id);
            if (Companys == null)
            {
                return NotFound();
            }

            _context.Companies.Remove(Companys);
            if (await _context.SaveChangesAsync() > 0)
            {
                return NoContent();
            }

            return BadRequest("Error removing resource");
        }

        private bool Exists(int id)
        {
            return _context.Companies.Any(e => e.Id == id);
        }
    }
}
