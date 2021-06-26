using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WScore.Models
{
    public class LoginModel
    {
        [Required]
        public string Mobile { get; set; }

        [Required]
        public string Password { get; set; }
        public string DeviceToken { get; set; }
    }
}
