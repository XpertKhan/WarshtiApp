using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Warshti.Panel.Models
{
    public class LoginViewModel
    {
        [Display(Name = "Username")]
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(150, ErrorMessage = "{0} should be {2} - {1} characters", MinimumLength = 8)]
        public string Username { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(50, ErrorMessage = "{0} should be {2} - {1} characters", MinimumLength = 8)]
        public string Password { get; set; }

        [Required]
        public bool RememberMe { get; set; }
    }
}
