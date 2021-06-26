using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Warshti.Entities;
using System.Linq.Dynamic.Core;
using Warshti.Panel.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Warshti.Entities.Maintenance;

namespace Warshti.Panel.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly WScoreContext _context;
        private readonly IMapper _mapper;

        public OrderController(WScoreContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            return View();
        }

        #region Load Data
        [HttpPost]
        public IActionResult LoadData()
        {
            var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
            // Skiping number of Rows count  
            var start = Request.Form["start"].FirstOrDefault();
            // Paging Length 10,20  
            var length = Request.Form["length"].FirstOrDefault();
            // Sort Column Name  
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            // Sort Column Direction ( asc ,desc)  
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            // Search Value from (Search box)  

            var searchValue = Request.Form["search[value]"].FirstOrDefault();

            //Paging Size (10,20,50,100)  
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;

            // Getting all Customer data  
            var tempData = _context.Orders
                                .Include(p => p.Service)
                                .Include(p => p.Workshop)
                                .AsQueryable();

            //Sorting  
            if (!(string.IsNullOrEmpty(sortColumn) ||
                string.IsNullOrEmpty(sortColumnDirection)))
            {
                tempData = tempData.OrderBy(sortColumn + " " + sortColumnDirection);
            }
            //Search  
            if (!string.IsNullOrEmpty(searchValue))
            {
                tempData = tempData.Where(m => m.Service.Description.ToUpper().Contains(searchValue.ToUpper()) ||
                                               m.Workshop.Name.ToUpper().Contains(searchValue.ToUpper()) ||
                                               m.OrderNumber.ToUpper().Contains(searchValue.ToUpper()));
            }

            //total number of rows count   
            recordsTotal = tempData.Count();
            //Paging   
            var data = tempData.Skip(skip).Take(pageSize).ToList();
            var temp = _mapper.Map<List<OrderModel>>(data);

            return Ok(
                new
                {
                    draw = draw,
                    recordsFiltered = recordsTotal,
                    recordsTotal = recordsTotal,
                    data = temp
                });
        }
        #endregion

        #region Create
        [HttpGet()]
        public IActionResult Create()
        {

            var servies = _context.Services.ToList();

            var users = _context.Users
                .Where(p => p.UserTypeId == (int)UserType.Workshop);
            var model = new OrderCreateViewModel()
            {
                CreationDate = DateTime.Now,
                ExpectedCompletionDate = DateTime.Now,
                Workshops = users.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Name }).ToList(),
                Services = servies.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Description }).ToList(),
            };

            return PartialView(model);
        }

        [HttpPost()]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderCreateViewModel model)
        {
            var users = _context.Users
                .Where(p => p.UserTypeId == (int)UserType.Workshop);

            model.Workshops = users.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Name }).ToList();
            var servies = _context.Services.ToList();

            model.Services = servies.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Description }).ToList();

            if (ModelState.IsValid)
            {

                var temp = new Order()
                {
                    CreationDate = model.CreationDate,
                    EstimatedPrice = model.EstimatedPrice,
                    ExpectedCompletionDate = model.ExpectedCompletionDate,
                    OrderProgress = 0,
                    ServiceId = model.ServiceId,
                    WorkshopId = model.WorkshopId,
                    OrderStatusId = 1,
                };


                await _context.AddAsync(temp);
                if (!(await _context.SaveChangesAsync() > 0))
                {
                    ModelState.AddModelError("", "Error while saving data");
                }
            }
            else
            {
                ModelState.AddModelError("", "Provide all required data to proceed");
            }

            return PartialView(model);
        }
        #endregion

        #region Update
        [HttpGet()]
        public IActionResult Update(int id)
        {
            var temp = _context.Orders.FirstOrDefault(p => p.Id == id);
            if (temp == null)
            {
                return NotFound();
            }

            var servies = _context.Services.ToList();

            var users = _context.Users
                .Where(p => p.UserTypeId == (int)UserType.Workshop);

            var model = new OrderUpdateViewModel()
            {
                Id = temp.Id,
                CreationDate = temp.CreationDate,
                EstimatedPrice = temp.EstimatedPrice,
                ExpectedCompletionDate = temp.ExpectedCompletionDate,
                ServiceId = temp.ServiceId,
                WorkshopId = temp.WorkshopId,
                Services = servies.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Description }).ToList(),
                Workshops = users.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Name }).ToList()
            };

            return PartialView(model);
        }

        [HttpPost()]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(OrderUpdateViewModel model)
        {
            var servies = _context.Services.ToList();

            var users = _context.Users
                .Where(p => p.UserTypeId == (int)UserType.Workshop);

            model.Services = servies.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Description }).ToList();
            model.Workshops = users.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Name }).ToList();

            if (ModelState.IsValid)
            {
                var temp = _context.Orders.FirstOrDefault(p => p.Id == model.Id);
                if (temp == null)
                {
                    return NotFound();
                }

                temp.CreationDate = model.CreationDate;
                temp.EstimatedPrice = model.EstimatedPrice;
                temp.ExpectedCompletionDate = model.ExpectedCompletionDate;
                temp.OrderProgress = model.OrderProgress;
                temp.ServiceId = model.ServiceId;
                temp.WorkshopId = model.WorkshopId;
                temp.OrderStatusId = 1;

                _context.Update(temp);
                if (!(await _context.SaveChangesAsync() > 0))
                {
                    ModelState.AddModelError("", "Error while saving data");
                }
            }
            else
            {
                ModelState.AddModelError("", "Provide all required data to proceed");
            }

            return View(model);
        }
        #endregion

        #region Accept Offer
        [HttpPost()]
        public async Task<IActionResult> AcceptOffer(int id)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(p => p.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            var status = false;
            var message = string.Empty;

            order.OrderStatusId = (int)OrderStatus.Accepted;
            order.OrderNumber = $"{DateTime.Now.Year}-{DateTime.Now.Month}-{new Random().Next(100000, 999999).ToString()}";

            _context.Update(order);
            if (!(await _context.SaveChangesAsync() > 0))
            {
                message = "Error while saving data";
                status = true;
            }
            else
            {
                message = "Order created successfully";
                status = true;
            }

            return Json(new { status, message });
        }
        #endregion

        #region Progress
        [HttpGet()]
        public IActionResult Progress(int id)
        {

            var temp = _context.Orders.FirstOrDefault(p => p.Id == id);
            if (temp == null)
            {
                return NotFound();
            }

            var model = new OrderProgressViewModel
            {
                Id = temp.Id,
                OrderStatusId = temp.OrderStatusId,
                Progress = temp.OrderProgress
            };

            return PartialView(model);
        }

        [HttpPost()]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Progress(OrderProgressViewModel model)
        {
            var temp = _context.Orders.FirstOrDefault(p => p.Id == model.Id);
            if (temp == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                switch (model.OrderStatusId)
                {
                    case (int)OrderStatus.Completed:
                        temp.CompletionDate = DateTime.Now;
                        temp.OrderStatusId = (int)OrderStatus.Completed;
                        temp.OrderProgress = model.Progress;
                        break;
                    case (int)OrderStatus.Declined:
                        temp.OrderStatusId = (int)OrderStatus.Declined;
                        temp.OrderProgress = model.Progress; break;
                    case (int)OrderStatus.InProgress:
                        temp.OrderStatusId = (int)OrderStatus.InProgress;
                        temp.OrderProgress = model.Progress;
                        break;
                    case (int)OrderStatus.Cancelled:
                        temp.OrderStatusId = (int)OrderStatus.Cancelled;
                        temp.OrderProgress = model.Progress;
                        break;
                }

                _context.Update(temp);
                if (!(await _context.SaveChangesAsync() > 0))
                {
                    ModelState.AddModelError("", "Error while saving data");
                }
            }
            else
            {
                ModelState.AddModelError("", "Provide all required data to proceed");
            }

            return PartialView(model);
        }
        #endregion

        #region Delete
        [HttpDelete()]
        public async Task<IActionResult> Delete(int id)
        {
            var temp = _context.Orders.FirstOrDefault(p => p.Id == id);
            if (temp == null)
            {
                return NotFound();
            }

            var status = false;
            var message = string.Empty;

            temp.OrderStatusId = (int)OrderStatus.Cancelled;

            _context.Update(temp);
            if (!(await _context.SaveChangesAsync() > 0))
            {
                message = "Error while saving data";
                status = true;
            }
            else
            {
                message = "Order cancelled successfully";
                status = true;
            }

            return Json(new { status, message });
        }
        #endregion
    }
}
