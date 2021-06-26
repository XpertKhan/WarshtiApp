using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Warshti.Entities;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using Warshti.Panel.Areas.Admin.Models;
using Warshti.Entities.Maintenance;
using Microsoft.Extensions.Logging;

namespace Warshti.Panel.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrderStepController : Controller
    {
        private readonly WScoreContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderStepController> _logger;

        public OrderStepController(WScoreContext context, 
            IMapper mapper,
            ILogger<OrderStepController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region Load Data
        [HttpPost]
        public IActionResult LoadData(int id)
        {
            _logger.LogInformation($"Id is {id}");

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
            var tempData = _context.OrderSteps
                                .Where(p => p.OrderId == id)
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
                tempData = tempData.Where(m => m.Title.ToUpper().Contains(searchValue.ToUpper()));
            }

            //total number of rows count   
            recordsTotal = tempData.Count();
            //Paging   
            var data = tempData.Skip(skip).Take(pageSize).ToList();
            var temp = _mapper.Map<List<OrderStepModel>>(data);

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
        public IActionResult Create(int orderId)
        {
            var order = _context.Orders.FirstOrDefault(p => p.Id == orderId);
            if (order == null)
            {
                return NotFound();
            }

            var model = new OrderStepCreateViewModel()
            {
                OrderId = order.Id,
                ActionDate = DateTime.Now
            };

            return PartialView(model);
        }

        [HttpPost()]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderStepCreateViewModel model)
        {
            var order = _context.Orders.FirstOrDefault(p => p.Id == model.OrderId);
            if (order == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var temp = new OrderStep()
                {
                    OrderId = model.OrderId,
                    ActionDate = model.ActionDate,
                    OrderStepStatus = model.OrderStepStatus,
                    Title = model.Title
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
            var temp = _context.OrderSteps.FirstOrDefault(p => p.Id == id);
            if (temp == null)
            {
                return NotFound();
            }

            var model = new OrderStepUpdateViewModel()
            {
                Id = temp.Id,
                Title = temp.Title,
                ActionDate = temp.ActionDate,
                OrderStepStatus = temp.OrderStepStatus
            };

            return PartialView(model);
        }

        [HttpPost()]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(OrderStepUpdateViewModel model)
        {

            if (ModelState.IsValid)
            {
                var temp = _context.OrderSteps.FirstOrDefault(p => p.Id == model.Id);
                if (temp == null)
                {
                    return NotFound();
                }

                temp.Title = model.Title;
                temp.ActionDate = model.ActionDate;
                temp.OrderStepStatus = model.OrderStepStatus;

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
    }
}
