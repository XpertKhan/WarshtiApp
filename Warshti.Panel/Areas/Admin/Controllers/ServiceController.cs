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
    public class ServiceController : Controller
    {
        private readonly WScoreContext _context;
        private readonly IMapper _mapper;

        public ServiceController(WScoreContext context, IMapper mapper)
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
            var tempData = _context.Services
                                .Include(p => p.Company)
                                .Include(p => p.Color)
                                .Include(p => p.Department)
                                .Include(p => p.Model)
                                .Include(p => p.PaymentMethod)
                                .Include(p => p.Transmission)
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
                tempData = tempData.Where(m => m.Color.Name.ToUpper().Contains(searchValue.ToUpper()) ||
                                               m.Company.Name.ToUpper().Contains(searchValue.ToUpper()) ||
                                               m.Department.Name.ToUpper().Contains(searchValue.ToUpper()) ||
                                               m.Model.Name.ToUpper().Contains(searchValue.ToUpper()) ||
                                               m.PaymentMethod.Name.ToUpper().Contains(searchValue.ToUpper()) ||
                                               m.Transmission.Name.ToUpper().Contains(searchValue.ToUpper()) ||
                                               m.User.Name.ToUpper().Contains(searchValue.ToUpper()));
            }

            //total number of rows count   
            recordsTotal = tempData.Count();
            //Paging   
            var data = tempData.Skip(skip).Take(pageSize).ToList();
            var temp = _mapper.Map<List<ServiceModel>>(data);

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
            var faults = _context.Faults
                                .Select(p =>
                                new ServiceFaultViewModel
                                {
                                    IsActive = false,
                                    Name = p.Name,
                                    FaultId = p.Id
                                })
                                .ToList();

            var companies = _context.Companies.ToList();
            var colors = _context.Colors.ToList();
            var models = _context.Models.ToList();
            var departments = _context.Departments.ToList();
            var paymentMethods = _context.PaymentMethods.ToList();
            var transmissions = _context.Transmissions.ToList();

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
            var model = new ServiceCreateViewModel()
            {
                Faults = faults,
                ServiceStatus = 1,
                Users = userList,
                Companies = companies.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Name }).ToList(),
                Colors = colors.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Name }).ToList(),
                Models = models.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Name }).ToList(),
                Departments = departments.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Name }).ToList(),
                PaymentMethods = paymentMethods.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Name }).ToList(),
                Transmissions = transmissions.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Name }).ToList()
            };

            return PartialView(model);
        }

        [HttpPost()]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServiceCreateViewModel model)
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

            var companies = _context.Companies.ToList();
            var colors = _context.Colors.ToList();
            var models = _context.Models.ToList();
            var departments = _context.Departments.ToList();
            var paymentMethods = _context.PaymentMethods.ToList();
            var transmissions = _context.Transmissions.ToList();

            model.Companies = companies.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Name }).ToList();
            model.Colors = colors.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Name }).ToList();
            model.Models = models.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Name }).ToList();
            model.Departments = departments.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Name }).ToList();
            model.PaymentMethods = paymentMethods.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Name }).ToList();
            model.Transmissions = transmissions.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Name }).ToList();

            if (ModelState.IsValid)
            {
                var temp = new Service()
                {
                    Description = model.Description,
                    UserId = model.UserId,
                    ColorId = model.ColorId,
                    CompanyId = model.CompanyId,
                    DepartmentId = model.DepartmentId,
                    ModelId = model.ModelId,
                    PaymentMethodId = model.PaymentMethodId,
                    TransmissionId = model.TransmissionId,
                    ServiceStatus = model.ServiceStatus
                };


                await _context.AddAsync(temp);
                if (await _context.SaveChangesAsync() > 0)
                {
                    foreach (var item in model.Faults)
                    {
                        if (item.IsActive)
                        {
                            var fault = new ServiceFault
                            {
                                FaultId = item.FaultId,
                                ServiceId = temp.Id
                            };

                            await _context.AddAsync(fault);
                        }
                    }
                    
                    if (!(await _context.SaveChangesAsync() > 0))
                    {
                        ModelState.AddModelError("", "Error while saving data");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Error while saving data");
                }
            }
            else
            {
                ModelState.AddModelError("", "Provide all required data to proceed");
            }

            var faults = _context.Faults
                                .Select(p =>
                                new ServiceFaultViewModel
                                {
                                    IsActive = false,
                                    Name = p.Name,
                                    FaultId = p.Id
                                })
                                .ToList();
            model.Faults = faults;

            return PartialView(model);
        }
        #endregion

        #region Update
        [HttpGet()]
        public IActionResult Update(int id)
        {
            var temp = _context.Services
                .FirstOrDefault(p => p.Id == id);
            if (temp == null)
            {
                return NotFound();
            }

            var faults = _context.Faults
                                .Select(p =>
                                new ServiceFaultViewModel
                                {
                                    IsActive = false,
                                    Name = p.Name,
                                    FaultId = p.Id
                                })
                                .ToList();

            var companies = _context.Companies.ToList();
            var colors = _context.Colors.ToList();
            var models = _context.Models.ToList();
            var departments = _context.Departments.ToList();
            var paymentMethods = _context.PaymentMethods.ToList();
            var transmissions = _context.Transmissions.ToList();

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

            foreach (var fault in faults)
            {
                var serviceFault = _context.ServiceFaults.FirstOrDefault(p => p.ServiceId == temp.Id && p.FaultId == fault.FaultId);
                if (serviceFault != null)
                {
                    fault.IsActive = true;
                }

            }

            var model = new ServiceUpdateViewModel()
            {
                Id = temp.Id,
                Description = temp.Description,
                Faults = faults,
                UserId = temp.UserId,
                ServiceStatus = temp.ServiceStatus,
                Users = userList,
                CompanyId = temp.CompanyId,
                Companies = companies.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Name }).ToList(),
                ColorId = temp.ColorId,
                Colors = colors.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Name }).ToList(),
                ModelId = temp.ModelId,
                Models = models.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Name }).ToList(),
                DepartmentId = temp.DepartmentId,
                Departments = departments.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Name }).ToList(),
                PaymentMethodId = temp.PaymentMethodId,
                PaymentMethods = paymentMethods.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Name }).ToList(),
                TransmissionId = temp.TransmissionId,
                Transmissions = transmissions.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Name }).ToList()
            };

            return PartialView(model);
        }

        [HttpPost()]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(ServiceUpdateViewModel model)
        {
            var temp = _context.Services
                .Include(p => p.ServiceFaults)
                    .FirstOrDefault(p => p.Id == model.Id);
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

            var companies = _context.Companies.ToList();
            var colors = _context.Colors.ToList();
            var models = _context.Models.ToList();
            var departments = _context.Departments.ToList();
            var paymentMethods = _context.PaymentMethods.ToList();
            var transmissions = _context.Transmissions.ToList();

            model.Companies = companies.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Name }).ToList();
            model.Colors = colors.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Name }).ToList();
            model.Models = models.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Name }).ToList();
            model.Departments = departments.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Name }).ToList();
            model.PaymentMethods = paymentMethods.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Name }).ToList();
            model.Transmissions = transmissions.Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Name }).ToList();

            if (ModelState.IsValid)
            {
                foreach (var serviceFault in temp.ServiceFaults)
                {
                    _context.Remove(serviceFault);
                }

                temp.UserId = model.UserId;
                temp.ColorId = model.ColorId;
                temp.CompanyId = model.CompanyId;
                temp.DepartmentId = model.DepartmentId;
                temp.Description = model.Description;
                temp.ModelId = model.ModelId;
                temp.PaymentMethodId = model.PaymentMethodId;
                temp.TransmissionId = model.TransmissionId;
                temp.ServiceStatus = model.ServiceStatus;

                List<ServiceFault> serviceFaults = new List<ServiceFault>();
                foreach (var fault in model.Faults)
                {
                    if (fault.IsActive)
                    {
                        serviceFaults.Add(new ServiceFault
                        {
                            FaultId = fault.FaultId,
                            ServiceId = temp.Id
                        });
                    }
                }
                temp.ServiceFaults = serviceFaults;
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

            var faults = _context.Faults
                                .Select(p =>
                                new ServiceFaultViewModel
                                {
                                    IsActive = false,
                                    Name = p.Name,
                                    FaultId = p.Id
                                })
                                .ToList();
            model.Faults = faults;

            return PartialView(model);
        }
        #endregion

        #region Delete
        [HttpDelete()]
        public async Task<IActionResult> Delete(int id)
        {
            var temp = await _context.Services
                .FirstOrDefaultAsync(p => p.Id == id);
            if (temp == null)
            {
                return NotFound();
            }

            var status = false;
            var message = string.Empty;

            // Cancelled: 3
            temp.ServiceStatus = (int)ServiceStatus.Cancelled;

            _context.Update(temp);
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
