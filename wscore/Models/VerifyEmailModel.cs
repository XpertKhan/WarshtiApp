using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WScore.Models
{
    public class VerifyEmailModel
    {
        [Required]
        public string Token { get; set; }
    }
}
