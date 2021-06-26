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
    [Route("api/ordersteps")]
    [ApiController]
   // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class OrderStepController : ControllerBase
    {
        private readonly WScoreContext _context;
        private readonly IMapper _mapper;

        public OrderStepController(WScoreContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/ordersteps
        [HttpGet("{orderId}")]
        public async Task<ActionResult<IEnumerable<OrderStepModel>>> Get(int orderId)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(p => p.Id == orderId);
            if (order == null)
            {
                return NotFound();
            }

            var temp = await _context.OrderSteps
                .Include(p => p.Order)
                .Where(p => p.OrderId == orderId)
                .ToListAsync();
            return Ok(_mapper.Map<IEnumerable<OrderStepModel>>(temp));
        }

        // GET: api/ordersteps/1
        [HttpGet("get-single/{id}")]
        public async Task<ActionResult<OrderStepModel>> GetSingle(int id)
        {
            var step = await _context.OrderSteps
                .Include(p => p.Order)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (step == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<OrderStepModel>(step));
        }

        // PUT: api/colors/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, OrderStepUpdateModel step)
        {
            var temp = await _context.OrderSteps.FirstOrDefaultAsync(p => p.Id == id);
            if (temp == null)
            {
                return BadRequest();
            }

            temp.ActionDate = step.ActionDate;
            temp.OrderStepStatus = step.OrderStepStatus;
            temp.Title = step.Title;

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
        public async Task<ActionResult<OrderStepModel>> Create([FromBody] OrderStepCreateModel step)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(p => p.Id == step.OrderId);
            if (order == null)
            {
                return NotFound();
            }

            var temp = new OrderStep
            {
                ActionDate = step.ActionDate,
                OrderStepStatus = step.OrderStepStatus,
                Title = step.Title,
                OrderId = step.OrderId
            };

            _context.OrderSteps.Add(temp);
            if (await _context.SaveChangesAsync() > 0)
            {
                return Created($"api/ordersteps/get-single/{temp.Id}",
                        _mapper.Map<OrderStepModel>(temp));
            }
            return BadRequest("Error creating resource");
        }

        // DELETE: api/colors/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var colors = await _context.OrderSteps.FindAsync(id);
            if (colors == null)
            {
                return NotFound();
            }

            _context.OrderSteps.Remove(colors);
            if (await _context.SaveChangesAsync() > 0)
            {
                return NoContent();
            }

            return BadRequest("Error removing resource");
        }

        private bool Exists(int id)
        {
            return _context.OrderSteps.Any(e => e.Id == id);
        }
    }
}
