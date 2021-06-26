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
using Warshti.Entities.Entities.Car;
using Warshti.Entities.WScore;
using WScore.Models;

namespace WScore.Controllers
{
    [Route("/api/carinformation")]
    [ApiController]
   // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class CarInformationController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly WScoreContext _context;

        public CarInformationController(WScoreContext context,
            IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        #region Read


        [HttpGet("{userId}")]
        public ActionResult GetInformation([FromRoute]int userId)
        {
            var model = _context.CarsInformation.Where(e => e.UserId == userId).FirstOrDefault();
            if (model == null)
            {
                return NotFound("Not an car information");
            }

            return Ok(model);
        }

        [HttpPost("{userId}")]
        public async Task<ActionResult> UpdateInformation([FromRoute] int userId, [FromBody] CarInformation carInformation)
        {
            CarInformation carInfoEntity;
            carInfoEntity = _context.CarsInformation.Where(e => e.UserId == userId).FirstOrDefault();
            if (carInfoEntity == null)
            {
                carInfoEntity = new CarInformation();
                carInfoEntity.UserId = userId;
                _context.Entry(carInfoEntity).State = EntityState.Added;
            }
            else
            {
                _context.Entry(carInfoEntity).State = EntityState.Modified;
            }
            carInfoEntity.Model = carInformation.Model;
            carInfoEntity.Company = carInformation.Company;
            carInfoEntity.CarTransmission = carInformation.CarTransmission;
            carInfoEntity.Color = carInformation.Color;
            await _context.SaveChangesAsync();
            return Ok();

            
        }
        #endregion
    }
}
