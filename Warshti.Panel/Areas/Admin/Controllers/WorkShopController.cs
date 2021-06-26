using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Warshti.Entities;
using Warshti.Panel.Areas.Admin.Models;
using WScore.Entities.Identity;
using System.Security.Cryptography;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Http;
using Warshti.Entities.Maintenance;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Musan.Panel.Areas.Admin.Models;

namespace Warshti.Panel.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class WorkShopController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IMapper _mapper;
        private readonly WScoreContext _context;

        public WorkShopController(UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IMapper mapper,
            WScoreContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region Load Data
        [HttpPost]
        public async Task<IActionResult> LoadData()
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
            var tempData = _context.Users
                                .Include(p => p.WorkShopInfo)
                                .Include(p => p.WorkShopImages)
                                .Where(p => p.UserTypeId == (int)UserType.Workshop)
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
                tempData = tempData.Where(m => m.Name.Contains(searchValue.ToUpper()) ||
                                               m.Email.Contains(searchValue.ToUpper()) ||
                                               m.UserName.Contains(searchValue.ToUpper()));
            }

            //total number of rows count   
            recordsTotal = tempData.Count();
            //Paging   
            var data = tempData.Skip(skip).Take(pageSize).ToList();
            var users = _mapper.Map<List<WorkShopModel>>(data);

            foreach (var user in users)
            {
                user.Roles = await GetRoles(user.Id);
            }

            return Ok(
                new
                {
                    draw = draw,
                    recordsFiltered = recordsTotal,
                    recordsTotal = recordsTotal,
                    data = users
                });
        }
        #endregion

        #region Roles Listing
        private async Task<List<string>> GetRoles(int userId)
        {
            List<string> Roles = new List<string>();
            List<Role> _roles = await _roleManager.Roles.ToListAsync();
            foreach (var role in _roles)
            {
                var status = await IsInRole(userId, role.Id);
                if (status == true)
                {
                    Roles.Add(role.Name);
                }
            }

            return Roles;
        }
        #endregion

        #region Check for Roles
        private async Task<bool> IsInRole(int userId, int roleId)
        {
            var user = _userManager.Users.FirstOrDefault(p => p.Id == userId);
            if (user != null)
            {
                var role = _roleManager.Roles.FirstOrDefault(p => p.Id == roleId);
                if (role != null)
                {
                    return await _userManager.IsInRoleAsync(user, role.Name);
                }

            }
            return false;
        }
        #endregion

        #region Create
        [HttpGet()]
        public IActionResult Create()
        {
            var model = new WorkShopCreateViewModel()
            {
            };

            return PartialView(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WorkShopCreateViewModel model, IFormFile Photo, List<IFormFile> Photos)
        {
            if (ModelState.IsValid)
            {
                var workShop = new User
                {
                    Created = DateTime.Now,
                    Email = model.Email,
                    IsActive = true,
                    Name = model.Name,
                    PhoneNumber = model.Mobile,
                    UserTypeId = (int)UserType.Workshop,
                    VerificationToken = RandomTokenString(),
                    UserName = model.Email
                };

                var result = await _userManager.CreateAsync(workShop, model.Password);
                if (result.Succeeded)
                {
                    var workShopInfo = new WorkShopInfo
                    {
                        CommercialRegister = model.WorkShopInfo.CommercialRegister,
                        ElectonicPaymentAccount = model.WorkShopInfo.ElectonicPaymentAccount,
                        Department = model.WorkShopInfo.Department,
                        Facility = model.WorkShopInfo.Facility,
                        Latitude = model.WorkShopInfo.Latitude,
                        Sponsor = model.WorkShopInfo.Sponsor,
                        Longitude = model.WorkShopInfo.Longitude,
                        WorkShopId = workShop.Id
                    };

                    if (Photo != null)
                    {
                        using (var target = new MemoryStream())
                        {
                            await Photo.CopyToAsync(target);
                            workShopInfo.Photo = target.ToArray();
                        }
                    }
                

                    List<WorkShopImage> workShopImages = new List<WorkShopImage>();

                    foreach (var workShopImage in Photos)
                    {
                        using (var target = new MemoryStream())
                        {
                            await workShopImage.CopyToAsync(target);

                            var image = new WorkShopImage
                            {
                                Photo = target.ToArray(),
                                WorkShopId = workShop.Id
                            };
                            workShopImages.Add(image);
                        }
                    }

                    await _context.AddAsync(workShopInfo);
                    await _context.AddRangeAsync(workShopImages);

                    if (!(await _context.SaveChangesAsync() > 0))
                    {
                        ModelState.AddModelError("", "Provide all required data to proceed");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Provide all required data to proceed");
                }
            }
            else
            {
                ModelState.AddModelError("", "Provide all required data to proceed");
            }
            return PartialView(model);
        }
        #endregion

        #region Update Workshop
        //[HttpGet()]
        //public IActionResult Update(int id)
        //{
        //    var temp = _context.Users
        //    .FirstOrDefault(p => p.Id == id);
           
        //    if (temp == null)
        //    {
        //        return NotFound();
        //    }

        //    var workShopInfo = _context.WorkShopInfos.Where(x => x.WorkShopId == temp.Id).FirstOrDefault();



        //    var model = new WorkshopUpdateViewModel();
        //    if (workShopInfo != null)
        //    {
        //        model.WorkShopInfo.CommercialRegister = workShopInfo.CommercialRegister ?? "";
        //        model.WorkShopInfo.ElectonicPaymentAccount = workShopInfo.ElectonicPaymentAccount ?? "";
        //        model.WorkShopInfo.Department = workShopInfo.Department ?? "";
        //        model.WorkShopInfo.Facility = workShopInfo.Facility ?? "";
        //        model.WorkShopInfo.Latitude = workShopInfo.Latitude ?? (double?)null;
        //        model.WorkShopInfo.Sponsor = workShopInfo.Sponsor ?? "";
        //        model.WorkShopInfo.Longitude = workShopInfo.Longitude ?? (double?)null;
        //    }
            
        //    model.Name= temp.Name ?? "";
        //    model.Email = temp.Email ?? "";
        //    model.Mobile = temp.PhoneNumber ?? "";


        //    return PartialView(model);
        //}

        //[HttpPost()]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Update(WorkshopUpdateViewModel model, List<IFormFile> Photos)
        //{
        //    try
        //    {
                 
 

        //        return View(model);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}
        #endregion
        private static string RandomTokenString()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            // convert random bytes to hex string
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }
    }
}
