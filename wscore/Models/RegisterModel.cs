using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Warshti.Entities;

namespace WScore.Models
{
    public class RegisterModel
    {
        [Required]
        public string Name { get; set; }

        public string Email { get; set; }
        
        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Password { get; set; }
        [Required]
        public UserType Type { get; set; }
    }
}
