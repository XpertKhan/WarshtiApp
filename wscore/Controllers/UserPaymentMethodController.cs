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
    [Route("api/userpaymentmethods")]
    [ApiController]
  //  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class UserPaymentMethodController : ControllerBase
    {
        private readonly WScoreContext _context;
        private readonly IMapper _mapper;

        public UserPaymentMethodController(WScoreContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/colors
        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<UserPaymentMethodModel>>> Get(int userId)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == userId);
            if (user == null)
            {
                return NotFound("No user found");
            }

            var temp = await _context.UserPaymentMethods
                .Include(p => p.User)
                .Include(p => p.PaymentMethod)
                .Where(p => p.UserId == user.Id)
                .ToListAsync();
            return Ok(_mapper.Map<IEnumerable<UserPaymentMethodModel>>(temp));
        }

        // GET: api/colors/5
        [HttpGet("/single/{id}")]
        public async Task<ActionResult<UserPaymentMethodModel>> GetSingle(int id)
        {
            var color = await _context.UserPaymentMethods
                .Include(p => p.User)
                .Include(p => p.PaymentMethod)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (color == null)
            {
                return NotFound("No user's payment method found");
            }

            return Ok(_mapper.Map<UserPaymentMethodModel>(color));
        }

        // PUT: api/colors/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UserPaymentMethodUpdateModel model)
        {


            var temp = await _context.UserPaymentMethods.FirstOrDefaultAsync(p => p.Id == id);
            if (temp == null)
            {
                return BadRequest();
            }


            temp.CardHolderName = model.CardHolderName;
            temp.CardNumber = model.CardNumber;
            temp.Cvc = model.Cvc;
            temp.ExpiryDate = model.ExpiryDate;
            temp.IsPreferred = model.IsPreferred;
            temp.PaymentMethodId = model.PaymentMethodId;

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
        [HttpPost("create/{userId}")]
        public async Task<ActionResult<UserPaymentMethodModel>> Create(int userId, UserPaymentMethodCreateModel model)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == userId);
            if (user == null)
            {
                return NotFound("No user found");
            }

            var temp = new UserPaymentMethod
            {
                CardHolderName = model.CardHolderName,
                CardNumber = model.CardNumber,
                Cvc = model.Cvc,
                ExpiryDate = model.ExpiryDate,
                IsPreferred = model.IsPreferred,
                PaymentMethodId = model.PaymentMethodId,
                UserId = user.Id
            };

            _context.UserPaymentMethods.Add(temp);
            if (await _context.SaveChangesAsync() > 0)
            {
                return Created($"api/userpaymentmethods/{temp.Id}",
                        _mapper.Map<UserPaymentMethodModel>(temp));
            }
            return BadRequest("Error creating resource");
        }

        // DELETE: api/colors/5
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var colors = await _context.UserPaymentMethods.FindAsync(id);
            if (colors == null)
            {
                return NotFound();
            }

            _context.UserPaymentMethods.Remove(colors);
            if (await _context.SaveChangesAsync() > 0)
            {
                return NoContent();
            }

            return BadRequest("Error removing resource");
        }

        private bool Exists(int id)
        {
            return _context.UserPaymentMethods.Any(e => e.Id == id);
        }
    }
}
