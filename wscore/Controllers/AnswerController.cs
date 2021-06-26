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
using Warshti.Entities.WScore;
using WScore.Models;

namespace WScore.Controllers
{
    [Route("/api/answers")]
    [ApiController]
   // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class AnswerController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly WScoreContext _context;

        public AnswerController(WScoreContext context,
            IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        

        #region Create
        [HttpPost()]
        public ActionResult Create([FromForm] AnswerCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var temp = new Answer
                {
                    AnswerText = model.AnswerText,
                    QuestionId = model.QuestionId,
                    UserId = model.UserId
                };

                return Ok();
            }

            return BadRequest(ModelState);
        }
        #endregion
    }
}
