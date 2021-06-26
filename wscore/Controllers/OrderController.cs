using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Warshti.Entities;
using Warshti.Entities.Maintenance;
using WScore.Entities.Identity;
using WScore.Models;

namespace WScore.Controllers
{
    [Route("api/orders")]
    [ApiController]
   // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class OrderController : Controller
    {
        private readonly WScoreContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderController> _logger;
        private readonly UserManager<User> _userManager;

        public OrderController(WScoreContext context,
            IMapper mapper,
            ILogger<OrderController> logger,
            UserManager<User> userManager)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _userManager = userManager;
        }

        [HttpGet()]
        public ActionResult Get()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            int uId = 0;
            if (identity != null)
            {
                // or
                string userId = identity.FindFirst("id").Value;
                int.TryParse(userId, out uId);

            }
            var model = _context.Orders
                            .Include(p => p.Service)
                            .Include(p => p.Workshop)
                            .Include(p => p.OrderSteps);

            return Ok(_mapper.Map<IEnumerable<OrderModel>>(model));
        }

        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            var model = _context.Orders
                            .Include(p => p.Service)
                            .Include(p => p.Workshop)
                            .Include(p => p.OrderSteps)
                            .FirstOrDefault(p => p.Id == id);
            if (model == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<OrderModel>(model));
        }

        [HttpGet("{userId}/get-orders")]
        public ActionResult GetOrders(int userId)
        {
            var model = _context.Orders
                            .Include(p => p.Service)
                            .Include(p => p.Workshop)
                            .Include(p => p.OrderSteps)
                            .Where(p => p.Service.UserId == userId && p.OrderStatusId != (int)OrderStatus.Offer)
                            .OrderByDescending(p => p.CreationDate);
            return Ok(_mapper.Map<IEnumerable<OrderModel>>(model));
        }

        [HttpGet("/api/orders/{userId}/get-offers")]
        public ActionResult GetUserAllOffers(int userId)
        {
            var model = _context.Orders
                            .Include(p => p.Service)
                            .Include(p => p.Workshop)
                            .Include(p => p.OrderSteps)
                            .Where(p =>
                                p.Service.UserId == userId &&
                                p.OrderStatusId == (int)OrderStatus.Offer).OrderByDescending(e=>e.CreationDate);
            return Ok(_mapper.Map<IEnumerable<OrderModel>>(model));
        }


        [HttpGet("/api/orders/{userId}/get-offers/{serviceId}")]
        public ActionResult GetOffers(int userId, int serviceId)
        {
            var model = _context.Orders
                            .Include(p => p.Service)
                            .Include(p => p.Workshop)
                            .Include(p => p.OrderSteps)
                            .Where(p => 
                                p.Service.UserId == userId && 
                                p.ServiceId == serviceId && 
                                p.OrderStatusId == (int)OrderStatus.Offer);
            return Ok(_mapper.Map<IEnumerable<OrderModel>>(model));
        }

        [HttpGet("/api/orders/{orderId}/accept-offers")]
        public async Task<ActionResult> AcceptOffer(int orderId)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(p => p.Id == orderId);
            if (order == null)
            {
                return NotFound();
            }

            order.OrderStatusId = (int)OrderStatus.Accepted;
            order.OrderNumber = $"{DateTime.Now.Year}-{DateTime.Now.Month}-{new Random().Next(100000, 999999).ToString()}";

            _context.Update(order);
            if (await _context.SaveChangesAsync() > 0)
            {
                return Ok(new { message = "Order has been placed successfully" });
            }

            return BadRequest("Error while saving resource");
        }

        [HttpPost("make-offer")]
        public async Task<IActionResult> Offer([FromBody] OfferCreateModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var offer = new Order
                    {
                        CompletionDate = null,
                        CreationDate = DateTime.Now,
                        ExpectedCompletionDate = model.ExpectedCompletionDate,
                        OrderProgress = 0,
                        OrderStatusId = 1,
                        ServiceId = model.ServiceId,
                        WorkshopId = model.WorkshopId,
                        EstimatedPrice = model.EstimatedPrice
                    };

                    _context.Orders.Add(offer);
                    if (await _context.SaveChangesAsync() > 0)
                    {
                        return Created($"/api/get/{offer.Id}",
                            _mapper.Map<OrderModel>(offer));
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error: {ex.Message}");
                }
            }

            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var order = await _context.Orders
                .Include(p => p.OrderSteps)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (order == null)
            {
                return NotFound("Not an order");
            }

            foreach (var step in order.OrderSteps)
            {
                _context.Remove(step);
            }

            _context.Orders.Remove(order);
            if (await _context.SaveChangesAsync() > 0)
            {
                return NoContent();
            }

            return BadRequest("Error removing resource");
        }



        


        [HttpPost("completeorder/{id}")]
        public async Task<IActionResult> CompleteOrder(int id, [FromBody]OrderCompleteModel model)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == id);
            order.Comments = model.Comments;
            order.OrderRating = model.Rating;
            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(new
            {
                message = "Order marked as completed"
            });
        }

        [HttpPut("updateorderflowstatus/{id}/{orderflowstatus_id}")]
        public async Task<IActionResult> UpdateOrderFlowStatus(int id, OrderFlowStatus orderflowstatus_id)
        {
            var order = await _context.Orders.Where(e => e.Id == id).FirstOrDefaultAsync();
            order.FlowStatus = orderflowstatus_id;
            _context.Entry(order).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(new
            {
                message = "Status Updated Successfull."
            });
        }

        [HttpGet("/api/orders/{orderId}/decline-offers")]
        public async Task<ActionResult> DeclineOffer(int orderId)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(p => p.Id == orderId);
            if (order == null)
            {
                return NotFound();
            }

            order.OrderStatusId = (int)OrderStatus.Declined;
            //order.OrderNumber = $"{DateTime.Now.Year}-{DateTime.Now.Month}-{new Random().Next(100000, 999999).ToString()}";

            _context.Update(order);
            if (await _context.SaveChangesAsync() > 0)
            {
                return Ok(new { message = "Order has been declined successfully" });
            }

            return BadRequest("Error while saving resource");
        }




        [HttpGet("getordersstatus/{userId}")]
        public async Task<IActionResult> GetOrderStatus (int userId)
        {
            var user = _context.Users.Where(e => e.Id == userId).FirstOrDefault();
            if (user.UserTypeId == 1)
            {
                var query = await (from o in _context.Orders
                                   join s in _context.Services on new { ServiceId = o.ServiceId, UserId = userId } equals new { ServiceId = s.Id, UserId = s.UserId }
                                   join u in _context.Users on o.WorkshopId equals u.Id
                                   where o.FlowStatus == OrderFlowStatus.WORKSHOP_DELIVERED_ORDER || o.FlowStatus == OrderFlowStatus.CLIENT_RATE_AND_COMMENT
                                   select new
                                   {
                                       o.Id,
                                       o.ServiceId,
                                       o.OrderNumber,
                                       o.FlowStatus,
                                       Workshopname = u.Name,
                                   }).FirstOrDefaultAsync();
                return Ok(query);
            }
            else
            {
                var query = await (from o in _context.Orders where o.WorkshopId == userId
                                   join s in _context.Services on o.ServiceId equals s.Id
                                   join u in _context.Users on s.UserId equals u.Id
                                   where o.FlowStatus == OrderFlowStatus.SET_RECEIVED_AMOUNT
                                   select new
                                   {
                                       o.Id,
                                       o.OrderNumber,
                                       o.ServiceId,
                                       o.FlowStatus,
                                       Clientname = u.Name
                                   }).FirstOrDefaultAsync();
                return Ok(query);

            }
        }

    }
}
