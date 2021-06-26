using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Warshti.Entities;
using Warshti.Entities.Maintenance;
using WScore.Models;

namespace WScore.Controllers
{
    [Route("/api/workshops")]
    [ApiController]
 //   [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class WorkShopInfoController : Controller
    {
        private readonly WScoreContext _context;
        private readonly IMapper _mapper;

        public WorkShopInfoController(WScoreContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #region Read
        [HttpGet("workshop-info/{workshopId}")]
        public async Task<IActionResult> Get(int workshopId)
        {
            var account = await _context.Users.FirstOrDefaultAsync(x => x.Id == workshopId && x.UserTypeId == (int)UserType.Workshop);

            if (account == null) return NotFound("Workshop not available");

            var model = _context.WorkShopInfos.FirstOrDefault(p => p.WorkShopId == account.Id);

            if (model == null)
            {
                return NotFound("No data found");
            }

            var images = _context.WorkShopImages.Where(p => p.WorkShopId == account.Id)
                .Select(p => p.Photo);

            var result = new WorkShopModel
            {
                user = account,
                WorkShopInfo = _mapper.Map<WorkShopInfoModel>(model),
                Pictures = images.ToList()
            };

            return Ok(result);
        }
        #endregion

        #region Create
        [HttpPost("create-info/{workshopId}")]
        public async Task<IActionResult> Create(int workshopId, [FromForm] WorkShopInfoCreateModel model)
        {
            var account = await _context.Users.FirstOrDefaultAsync(
                x => x.Id == workshopId &&
                     x.UserTypeId == (int)UserType.Workshop);
            account.Name = model.user.Name;
            account.PhoneNumber = model.user.PhoneNumber;
            account.Email = model.user.Email;
            _context.Entry(account).State = EntityState.Modified;
            if (account == null) return BadRequest("Verification failed");

            if (model.Photo != null)
            {
                if (!(model.Photo.Length > 0))
                {
                    return BadRequest("Image is reuqired");
                }
            }

            var entry = _context.WorkShopInfos.FirstOrDefault(p => p.WorkShopId == account.Id);

            if (entry != null)
            {
                return BadRequest("Resource already exists");
            }

            var temp = new WorkShopInfo
            {
                CommercialRegister = model.CommercialRegister,
                Department = model.Department,
                ElectonicPaymentAccount = model.ElectonicPaymentAccount,
                Facility = model.Facility,
                Latitude = model.Latitude,
                Longitude = model.Longitude,
                Sponsor = model.Sponsor,
                WorkShopId = account.Id
            };

            if (model.Photo != null)
            {
                using (var target = new MemoryStream())
                {
                    model.Photo.CopyTo(target);
                    temp.Photo = target.ToArray();
                }
                
            }
            _context.Add(temp);
            _context.SaveChanges();

            if (true)
            {

                if (model.Pictures != null)
                {
                    List<WorkShopImage> images = new List<WorkShopImage>();

                    foreach (var photo in model.Pictures)
                    {
                        if (photo.Length > 0)
                        {
                            using (var target = new MemoryStream())
                            {
                                photo.CopyTo(target);
                                images.Add(new WorkShopImage
                                {
                                    Photo = target.ToArray(),
                                    WorkShopId = workshopId
                                });
                            }
                        }
                    }


                    if (images.Any())
                    {
                        _context.AddRange(images);
                        _context.SaveChanges();
                      
                    }
                }
            }
            
            return Created($"api/workshops/workshop-info",
            _mapper.Map<WorkShopInfoModel>(temp));
            

            return BadRequest("Error while creating resource");
        }
        #endregion

        #region Update
        [HttpPost("update-info/{workshopId}")]
        public async Task<IActionResult> Update(int workshopId, [FromForm] WorkShopInfoUpdateModel model)
        {
            var account = await _context.Users.FirstOrDefaultAsync(x => x.Id == workshopId &&
                    x.UserTypeId == (int)UserType.Workshop);

            if (account == null) return BadRequest("Workshop data not found");
            account.Name = model.user.Name;
            account.PhoneNumber = model.user.PhoneNumber;
            account.Email = model.user.Email;
            _context.Entry(account).State = EntityState.Modified;
            

            var entry = _context.WorkShopInfos.FirstOrDefault(p => p.WorkShopId == account.Id);

            if (entry == null)
            {
                return NotFound("No data for workshop info");
            }

            entry.CommercialRegister = model.CommercialRegister;
            entry.Department = model.Department;
            entry.ElectonicPaymentAccount = model.ElectonicPaymentAccount;
            entry.Facility = model.Facility;
            entry.Latitude = model.Latitude;
            entry.Longitude = model.Longitude;
            entry.Sponsor = model.Sponsor;
            if (model.Photo != null)
            {
                using (var target = new MemoryStream())
                {
                    model.Photo.CopyTo(target);
                    entry.Photo = target.ToArray();
                }
                
            }

            _context.Update(entry);
            _context.SaveChanges();
            if (true)
            {
                if (model.Pictures != null)
                {
                    if (model.Pictures.Count > 0)
                    {
                        var images = _context.WorkShopImages.Where(p => p.WorkShopId == account.Id);
                        if (images != null)
                        {
                            _context.RemoveRange(images);
                            _context.SaveChanges();
                        }

                        List<WorkShopImage> imageTemp = new List<WorkShopImage>();

                        foreach (var photo in model.Pictures)
                        {
                            if (photo.Length > 0)
                            {
                                using (var target = new MemoryStream())
                                {
                                    photo.CopyTo(target);
                                    imageTemp.Add(new WorkShopImage
                                    {
                                        Photo = target.ToArray(),
                                        WorkShopId = workshopId
                                    });
                                }

                            }
                        }

                        if (imageTemp.Any())
                        {
                            _context.AddRange(imageTemp);
                            _context.SaveChanges();
                        }
                    }
                }

            }


            return Ok();
        }
        #endregion
    }
}
