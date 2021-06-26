using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;


using AutoMapper;
using WScore.Models;

using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using WScore.Entities.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Warshti.Entities;
using Warshti.Entities.Maintenance;
using Warshti.Entities.Entities.Maintenance;
using System.IO;

namespace wscore.Controllers
{
    [Route("api/services")]
    [ApiController]
   // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ServiceController : ControllerBase
    {
        private readonly WScoreContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<ServiceController> _logger;
        private readonly UserManager<User> _userManager;

        public ServiceController(WScoreContext context,
            IMapper mapper,
            ILogger<ServiceController> logger,
            UserManager<User> userManager)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _userManager = userManager;
        }

        // GET: api/Services
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServiceModelCustom>>> Get()
        {
            var temp = await _context.Services
                .Include(p => p.User)
                .Include(p => p.Color)
                .Include(p => p.Company)
                .Include(p => p.Department)
                .Include(p => p.Model)
                .Include(p => p.PaymentMethod)
                .Include(p => p.Transmission)
                .Include(p => p.ServiceFaults).ThenInclude(p => p.Fault)
                .Include(p => p.ServiceFaults).ThenInclude(p => p.Service)
                .ToListAsync();
            return Ok(_mapper.Map<IEnumerable<ServiceModelCustom>>(temp));
        }

        // GET: api/Services/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceModelCustom>> Get(int id)
        {
            var Service = await _context.Services
                .Include(p => p.User)
                .Include(p => p.Color)
                .Include(p => p.Company)
                .Include(p => p.Department)
                .Include(p => p.Model)
                .Include(p => p.PaymentMethod)
                .Include(p => p.Transmission)
                .Include(p => p.ServiceFaults).ThenInclude(p => p.Fault)
                .Include(p => p.ServiceFaults).ThenInclude(p => p.Service)
                .Include(p => p.ServiceImages)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (Service == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ServiceModelCustom>(Service));
        }

        // PUT: api/Services/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ServiceUpdateModel Service)
        {
            var temp = await _context.Services.FirstOrDefaultAsync(p => p.Id == id);
            if (temp == null)
            {
                return NotFound("No service found");
            }

            temp.ColorId = Service.ColorId;
            temp.CompanyId = Service.CompanyId;
            temp.DepartmentId = Service.DepartmentId;
            temp.Description = Service.Description;
            temp.ModelId = Service.ModelId;
            temp.PaymentMethodId = Service.PaymentMethodId;
            temp.TransmissionId = Service.TransmissionId;

            _context.Entry(temp).State = EntityState.Modified;

            try
            {
                if (await _context.SaveChangesAsync() > 0)
                {
                    var faults = _context.ServiceFaults.Where(p => p.ServiceId == temp.Id);
                    if (faults != null)
                    {
                        _context.ServiceFaults.RemoveRange(faults);
                        if (await _context.SaveChangesAsync() > 0)
                        {
                            List<ServiceFault> serviceFaults = new List<ServiceFault>();
                            foreach (var fault in Service.Faults)
                            {
                                var serviceFault = new ServiceFault
                                {
                                    FaultId = fault,
                                    ServiceId = temp.Id
                                };

                                serviceFaults.Add(serviceFault);
                            }

                            _context.ServiceFaults.AddRange(serviceFaults);
                            await _context.SaveChangesAsync();
                        }
                    }
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Exists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Services
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("create/{userId}")]
        public async Task<ActionResult<ServiceModelCustom>> Create(int userId, [FromForm] ServiceCreateModel Service)
        {
            var user = await _context.Users.FirstOrDefaultAsync(p => p.Id == userId);
            if (user == null)
            {
                return NotFound("No user found");
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var temp = new Service
                    {
                        ColorId = Service.ColorId,
                        CompanyId = Service.CompanyId,
                        DepartmentId = Service.DepartmentId,
                        Description = Service.Description,
                        ModelId = Service.ModelId,
                        PaymentMethodId = Service.PaymentMethodId,
                        TransmissionId = Service.TransmissionId,
                        UserId = user.Id
                    };

                    await _context.Services.AddAsync(temp);
                    await _context.SaveChangesAsync();

                    List<ServiceFault> serviceFaults = new List<ServiceFault>();
                    foreach (var fault in Service.Faults)
                    {
                        var serviceFault = new ServiceFault
                        {
                            FaultId = fault,
                            ServiceId = temp.Id
                        };

                        serviceFaults.Add(serviceFault);
                    }

                    await _context.ServiceFaults.AddRangeAsync(serviceFaults);
                    if (Service.Photos != null)
                    {
                        List<ServiceImage> images = new List<ServiceImage>();
                        foreach (var item in Service.Photos)
                        {
                            using (var target = new MemoryStream())
                            {
                                item.CopyTo(target);
                                images.Add(new ServiceImage
                                {
                                    Photo = target.ToArray(),
                                    ServiceId = temp.Id
                                });
                            }
                        }
                        await _context.ServiceImages.AddRangeAsync(images);
                    }

                    

                    if (await _context.SaveChangesAsync() > 0)
                    {
                        await transaction.CommitAsync();
                        _logger.LogInformation("Resource created");

                        return Created($"api/Services/{temp.Id}",
                            _mapper.Map<ServiceModelCustom>(temp));
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error: {ex.Message}");
                    await transaction.RollbackAsync();
                }
            }

            return BadRequest("Error creating resource");
        }

        // DELETE: api/Services/5
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var temp = await _context.Services.FindAsync(id);
            if (temp == null)
            {
                return NotFound("No service found");
            }

            var faults = _context.ServiceFaults.Where(p => p.ServiceId == temp.Id);
            if (faults != null)
            {
                _context.ServiceFaults.RemoveRange(faults);
                _context.Services.Remove(temp);

                if (await _context.SaveChangesAsync() > 0)
                {
                    return NoContent();
                }
            }
            return BadRequest("Error removing resource");
        }

        private bool Exists(int id)
        {
            return _context.Services.Any(e => e.Id == id);
        }
    }
}
