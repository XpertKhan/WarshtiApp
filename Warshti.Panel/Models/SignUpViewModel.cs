using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Warshti.Panel.Models
{
    public class SignUpViewModel
    {
        [Display(Name = "Name")]
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(100, ErrorMessage = "{0} should be {2} - {1} characters", MinimumLength = 3)]
        public string Name { get; set; }

        [Display(Name = "Username")]
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(25, ErrorMessage = "{0} should be {2} - {1} characters", MinimumLength = 8)]
        public string Username { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(25, ErrorMessage = "{0} should be {2} - {1} characters", MinimumLength = 8)]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(25, ErrorMessage = "{0} should be {2} - {1} characters", MinimumLength = 8)]
        [Compare("Password", ErrorMessage = "Password must be same to proceed")]
        public string ConfirmPassword { get; set; }
    }
}
