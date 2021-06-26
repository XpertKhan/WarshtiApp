using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WScore.Entities.Identity;
using System.Linq.Dynamic.Core;
using Warshti.Panel.Areas.Admin.Models;
using AutoMapper;
using Warshti.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace Warshti.Panel.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IMapper _mapper;

        public UserController(UserManager<User> userManager, 
            RoleManager<Role> roleManager,
            IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
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
            var tempData = (from tempUser in _userManager.Users
                            where tempUser.UserTypeId == (int)UserType.Client
                            orderby tempUser.Name
                            select tempUser).AsQueryable();

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
            var users = _mapper.Map<List<UserModel>>(data);

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
            List<Role> _roles= await _roleManager.Roles.ToListAsync();
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
            var roles = _roleManager.Roles
                                    .Select(p => new UserRoleViewModel { IsActive = false, Name = p.Name, RoleId = p.Id })
                                    .ToList();

            var model = new UserCreateViewModel()
            {
                Roles = roles
            };

            return PartialView(model);
        }

        [HttpPost()]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserCreateViewModel model)
        {
            var roles = _roleManager.Roles
                                    .Select(p => new UserRoleViewModel { IsActive = false, Name = p.Name, RoleId = p.Id })
                                    .ToList();

            if (ModelState.IsValid)
            {
                var user = new User()
                {
                    Created = DateTime.Now,
                    Email = model.Email,
                    Name = model.Name,
                    UserName = model.Email,
                    PhoneNumber = model.Mobile,
                    IsActive = true,
                    UserTypeId = (int)UserType.Client,
                    VerificationToken = RandomTokenString()
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Error while saving data");
                }
                else
                {
                    foreach (var role in model.Roles)
                    {
                        if (role.IsActive)
                        {
                            var option = await _userManager.AddToRoleAsync(user, role.Name);
                            if (!option.Succeeded)
                            {
                                ModelState.AddModelError("", "Error while saving data");
                            }
                        }
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Provide all required data to proceed");
            }

            model.Roles = roles;

            return PartialView(model);
        }
        #endregion

        #region Update
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(p => p.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            var roles = _roleManager.Roles
                                    .Select(p => new UserRoleViewModel { IsActive = false, Name = p.Name, RoleId = p.Id })
                                    .ToList();

            foreach (var item in roles)
            {
                var IsAssigned = await IsInRole(id, item.RoleId);
                if (IsAssigned)
                {
                    item.IsActive = true;
                }
            }

            var model = new UserUpdateViewModel
            {
                Email = user.Email,
                Name = user.Name,
                Id = user.Id,
                Mobile = user.PhoneNumber,
                Roles = roles
            };

            return PartialView(model);
        }

        [HttpPost()]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UserUpdateViewModel model)
        {
            var roles = _roleManager.Roles
                                    .Select(p => new UserRoleViewModel { IsActive = false, Name = p.Name, RoleId = p.Id })
                                    .ToList();

            if (ModelState.IsValid)
            {
                var user = await _userManager.Users.FirstOrDefaultAsync(p => p.Id == model.Id);
                if (user == null)
                {
                    return NotFound();
                }

                user.Name = model.Name;
                user.Email = model.Email;
                user.PhoneNumber = model.Mobile;

                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Error while saving data");
                }
                else
                {
                    foreach (var item in model.Roles)
                    {
                        var role = _roleManager.Roles.FirstOrDefault(p => p.Id == item.RoleId);
                        if (role != null)
                        {
                            var IsAssigned = await IsInRole(user.Id, role.Id);
                            switch (IsAssigned)
                            {
                                case true:
                                    if (!item.IsActive)
                                    {
                                        //TODO: remove from role
                                        await _userManager.RemoveFromRoleAsync(user, role.Name);
                                    }
                                    break;
                                case false:
                                    if (item.IsActive)
                                    {
                                        //TODO: add to role
                                        await _userManager.AddToRoleAsync(user, role.Name);
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Provide all required data to proceed");
            }

            model.Roles = roles;

            return PartialView(model);
        }
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
