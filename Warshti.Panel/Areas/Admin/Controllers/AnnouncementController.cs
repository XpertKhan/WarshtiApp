using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Warshti.Entities;
using System.Linq.Dynamic.Core;
using Warshti.Panel.Areas.Admin.Models;
using Warshti.Entities.WScore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Warshti.Panel.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AnnouncementController : Controller
    {
        private readonly WScoreContext _context;
        private readonly IMapper _mapper;

        public AnnouncementController(WScoreContext context, IMapper mapper)
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
            var tempData = _context.Announcements
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
                tempData = tempData.Where(m => m.Title.ToUpper().Contains(searchValue.ToUpper()) ||
                                               m.Description.ToUpper().Contains(searchValue.ToUpper()) ||
                                               m.Detail.ToUpper().Contains(searchValue.ToUpper()) ||
                                               m.User.Name.ToUpper().Contains(searchValue.ToUpper()));
            }

            //total number of rows count   
            recordsTotal = tempData.Count();
            //Paging   
            var data = tempData.Skip(skip).Take(pageSize).ToList();
            var temp = _mapper.Map<List<AnnouncementModel>>(data);

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
            var users = _context.Users.Where(p => p.UserTypeId == (int)UserType.Workshop);

            var model = new AnnouncementCreateViewModel()
            {
                Users = users.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                })
            };

            return PartialView(model);
        }

        [HttpPost()]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AnnouncementCreateViewModel model, List<IFormFile> Photos)
        {
            var users = _context.Users.Where(p => p.UserTypeId == (int)UserType.Workshop);
            model.Users = users.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Name
            });

            if (ModelState.IsValid)
            {
                List<AnnouncementImage> announcementImages = new List<AnnouncementImage>();

                foreach (var photo in Photos)
                {
                    using (var target = new MemoryStream())
                    {
                        await photo.CopyToAsync(target);

                        var image = new AnnouncementImage
                        {
                            Photo = target.ToArray()
                        };
                        announcementImages.Add(image);
                    }
                }

                var temp = new Announcement()
                {
                    Title = model.Title,
                    Description = model.Description,
                    Detail = model.Detail,
                    UserId = model.UserId,
                    AnnouncementImages = announcementImages
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
            var temp = _context.Announcements
                .Include(p => p.AnnouncementImages)
                .FirstOrDefault(p => p.Id == id);

            if (temp == null)
            {
                return NotFound();
            }

            var users = _context.Users.Where(p => p.UserTypeId == (int)UserType.Workshop);

            var model = new AnnouncementUpdateViewModel()
            {
                Id = temp.Id,
                Title = temp.Title,
                Description = temp.Description,
                Detail = temp.Detail,
                Users = users.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                }),
                UserId = temp.UserId,
                AnnouncementImages = _mapper.Map<List<AnnouncementImageModel>>(temp.AnnouncementImages)
            };

            return PartialView(model);
        }

        [HttpPost()]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(AnnouncementUpdateViewModel model, List<IFormFile> Photos)
        {
            try {
                if (ModelState.IsValid)
                {
                    var temp = _context.Announcements
                        .Include(p => p.AnnouncementImages)
                        .FirstOrDefault(p => p.Id == model.Id);
                    if (temp == null)
                    {
                        return NotFound();
                    }

                    foreach (var item in temp.AnnouncementImages)
                    {
                        _context.Remove(item);
                    }

                    var users = _context.Users.Where(p => p.UserTypeId == (int)UserType.Workshop);

                    model.Users = users.Select(p => new SelectListItem
                    {
                        Value = p.Id.ToString(),
                        Text = p.Name
                    });

                    List<AnnouncementImage> announcementImages = new List<AnnouncementImage>();

                    foreach (var photo in Photos)
                    {
                        using (var target = new MemoryStream())
                        {
                            await photo.CopyToAsync(target);

                            var image = new AnnouncementImage
                            {
                                Photo = target.ToArray()
                            };
                            announcementImages.Add(image);
                        }
                    }

                    temp.Title = model.Title;
                    temp.Detail = model.Detail;
                    temp.Description = model.Description;
                    temp.UserId = model.UserId;
                    temp.AnnouncementImages = announcementImages;

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
            catch(Exception ex)
            {
                throw ex;
            }
           
        }
        #endregion

        #region Delete
        [HttpDelete()]
        public async Task<IActionResult> Delete(int id)
        {
            var temp = await _context.Announcements
                .Include(p => p.AnnouncementImages)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (temp == null)
            {
                return NotFound();
            }

            var status = false;
            var message = string.Empty;

            foreach (var item in temp.AnnouncementImages)
            {
                _context.Remove(item);
            }

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
