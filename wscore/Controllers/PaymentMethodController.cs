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
    [Route("api/paymentmethods")]
    [ApiController]
   // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PaymentMethodController : ControllerBase
    {
        private readonly WScoreContext _context;
        private readonly IMapper _mapper;

        public PaymentMethodController(WScoreContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/PaymentMethods
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentMethodModel>>> Get()
        {
            var temp = await _context.PaymentMethods.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<PaymentMethodModel>>(temp));
        }

        // GET: api/PaymentMethods/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentMethodModel>> Get(int id)
        {
            var PaymentMethod = await _context.PaymentMethods.FindAsync(id);

            if (PaymentMethod == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PaymentMethodModel>(PaymentMethod));
        }

        // PUT: api/PaymentMethods/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, PaymentMethodUpdateModel PaymentMethod)
        {
            var temp = await _context.PaymentMethods.FirstOrDefaultAsync(p => p.Id == id);
            if (temp == null)
            {
                return BadRequest();
            }


            temp.Name = PaymentMethod.Name;

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

        // POST: api/PaymentMethods
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<PaymentMethodModel>> Create(PaymentMethodCreateModel PaymentMethod)
        {
            var temp = new PaymentMethod
            {
                Name = PaymentMethod.Name
            };

            _context.PaymentMethods.Add(temp);
            if (await _context.SaveChangesAsync() > 0)
            {
                return Created($"api/paymentmethods/{temp.Id}",
                        _mapper.Map<PaymentMethodModel>(temp));
            }
            return BadRequest("Error creating resource");
        }

        // DELETE: api/PaymentMethods/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var PaymentMethods = await _context.PaymentMethods.FindAsync(id);
            if (PaymentMethods == null)
            {
                return NotFound();
            }

            _context.PaymentMethods.Remove(PaymentMethods);
            if (await _context.SaveChangesAsync() > 0)
            {
                return NoContent();
            }

            return BadRequest("Error removing resource");
        }

        private bool Exists(int id)
        {
            return _context.PaymentMethods.Any(e => e.Id == id);
        }
    }
}
