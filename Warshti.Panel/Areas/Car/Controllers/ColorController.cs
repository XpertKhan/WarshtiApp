using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Warshti.Entities;
using System.Linq.Dynamic.Core;
using Warshti.Panel.Areas.Car.Models;
using Warshti.Entities.Car;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Warshti.Panel.Areas.Car.Controllers
{
    [Area("Car")]
    [Authorize]
    public class ColorController : Controller
    {
        private readonly WScoreContext _context;

        public ColorController(WScoreContext context)
        {
            _context = context;
        }

        [HttpGet()]
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
            var tempData = _context.Colors.AsQueryable();

            //Sorting  
            if (!(string.IsNullOrEmpty(sortColumn) ||
                string.IsNullOrEmpty(sortColumnDirection)))
            {
                tempData = tempData.OrderBy(sortColumn + " " + sortColumnDirection);
            }
            //Search  
            if (!string.IsNullOrEmpty(searchValue))
            {
                tempData = tempData.Where(m => m.Name.Contains(searchValue.ToUpper()));
            }

            //total number of rows count   
            recordsTotal = tempData.Count();
            //Paging   
            var data = tempData.Skip(skip).Take(pageSize).ToList();
            //var users = _mapper.Map<List<UserModel>>(data);

            return Ok(
                new
                {
                    draw = draw,
                    recordsFiltered = recordsTotal,
                    recordsTotal = recordsTotal,
                    data = data
                });
        }
        #endregion

        #region Create
        [HttpGet()]
        public IActionResult Create()
        {
            var model = new ColorCreateViewModel()
            {
            };

            return PartialView(model);
        }

        [HttpPost()]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ColorCreateViewModel model)
        {

            if (ModelState.IsValid)
            {
                var temp = new Color()
                {
                    Name = model.Name
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
            var temp = _context.Colors.FirstOrDefault(p => p.Id == id);
            if (temp == null)
            {
                return NotFound();
            }

            var model = new ColorUpdateViewModel()
            {
                Id = temp.Id,
                Name = temp.Name
            };

            return PartialView(model);
        }

        [HttpPost()]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(ColorUpdateViewModel model)
        {

            if (ModelState.IsValid)
            {
                var temp = _context.Colors.FirstOrDefault(p => p.Id == model.Id);
                if (temp == null)
                {
                    return NotFound();
                }

                temp.Name = model.Name;

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
            var temp = await _context.Colors.FirstOrDefaultAsync(p => p.Id == id);
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
