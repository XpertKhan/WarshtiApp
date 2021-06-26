using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WScore.Entities.Identity;

namespace WScore.Models
{
    public class AnswerModel
    {
       public string AnswerText { get; set; }
        public User user { get; set; }

    }
}
