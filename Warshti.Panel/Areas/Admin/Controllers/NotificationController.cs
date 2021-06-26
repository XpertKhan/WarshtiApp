using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Warshti.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Warshti.Panel.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using Warshti.Entities.WScore;

namespace Warshti.Panel.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class NotificationController : Controller
    {
        private readonly WScoreContext _context;
        private readonly IMapper _mapper;

        public NotificationController(WScoreContext context, IMapper mapper)
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
            var tempData = _context.Notifications
                                .Include(p => p.User)
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
                tempData = tempData.Where(m => m.Message.ToUpper().Contains(searchValue.ToUpper()) ||
                                               m.User.Name.ToUpper().Contains(searchValue.ToUpper()));
            }

            //total number of rows count   
            recordsTotal = tempData.Count();
            //Paging   
            var data = tempData.Skip(skip).Take(pageSize).ToList();
            var temp = _mapper.Map<List<NotificationModel>>(data);

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
            var users = _context.Users;
            List<SelectListItem> userList = new List<SelectListItem>();

            foreach (var user in users)
            {
                var role = string.Empty;
                switch (user.UserTypeId)
                {
                    case (int)UserType.Client:
                        role = "Client";
                        break;
                    case (int)UserType.Workshop:
                        role = "WorkShop";
                        break;
                }

                userList.Add(new SelectListItem()
                {
                    Value = user.Id.ToString(),
                    Text = $"({role}) - {user.Name}"
                });
            }
            var model = new NotificationCreateViewModel()
            {
                Users = userList
            };

            return PartialView(model);
        }

        [HttpPost()]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NotificationCreateViewModel model)
        {
            var users = _context.Users;
            List<SelectListItem> userList = new List<SelectListItem>();

            foreach (var user in users)
            {
                var role = string.Empty;
                switch (user.UserTypeId)
                {
                    case (int)UserType.Client:
                        role = "Client";
                        break;
                    case (int)UserType.Workshop:
                        role = "WorkShop";
                        break;
                }

                userList.Add(new SelectListItem()
                {
                    Value = user.Id.ToString(),
                    Text = $"({role}) - {user.Name}"
                });
            }

            model.Users = userList;

            if (ModelState.IsValid)
            {

                var temp = new Notification()
                {
                    Message = model.Message,
                    NotificationTime = DateTime.Now,
                    UserId = model.UserId,
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
            var temp = _context.Notifications.FirstOrDefault(p => p.Id == id);
            if (temp == null)
            {
                return NotFound();
            }

            var users = _context.Users;
            List<SelectListItem> userList = new List<SelectListItem>();

            foreach (var user in users)
            {
                var role = string.Empty;
                switch (user.UserTypeId)
                {
                    case (int)UserType.Client:
                        role = "Client";
                        break;
                    case (int)UserType.Workshop:
                        role = "WorkShop";
                        break;
                }

                userList.Add(new SelectListItem()
                {
                    Value = user.Id.ToString(),
                    Text = $"({role}) - {user.Name}"
                });
            }

            var model = new NotificationUpdateViewModel()
            {
                Id = temp.Id,
                Message = temp.Message,
                UserId = temp.UserId,
                Users = userList
            };

            return PartialView(model);
        }

        [HttpPost()]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(NotificationUpdateViewModel model)
        {

            if (ModelState.IsValid)
            {
                var temp = _context.Notifications.FirstOrDefault(p => p.Id == model.Id);
                if (temp == null)
                {
                    return NotFound();
                }

                var users = _context.Users;
                List<SelectListItem> userList = new List<SelectListItem>();

                foreach (var user in users)
                {
                    var role = string.Empty;
                    switch (user.UserTypeId)
                    {
                        case (int)UserType.Client:
                            role = "Client";
                            break;
                        case (int)UserType.Workshop:
                            role = "WorkShop";
                            break;
                    }

                    userList.Add(new SelectListItem()
                    {
                        Value = user.Id.ToString(),
                        Text = $"({role}) - {user.Name}"
                    });
                }
                
                model.Users = userList;

                temp.Message = model.Message;
                temp.UserId = model.UserId;
                temp.NotificationTime = DateTime.Now;

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

        #region Delete
        [HttpDelete()]
        public async Task<IActionResult> Delete(int id)
        {
            var temp = await _context.Notifications.FirstOrDefaultAsync(p => p.Id == id);
            if (temp == null)
            {
                return NotFound();
            }

            var status = false;
            var message = string.Empty;


            _context.Remove(temp);
            if (!(await _context.SaveChangesAsync() > 0))
            {
                message = "Error while saving data";
                status = true;
            }
            else
            {
                message = "Record removed successfully";
                status = true;
            }

            return Json(new { status, message });
        }
        #endregion
    }
}
