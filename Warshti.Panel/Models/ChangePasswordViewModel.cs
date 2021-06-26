using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Warshti.Panel.Models
{
    public class ChangePasswordViewModel
    {
        [Required]
        public int UserId { get; set; }

        [Display(Name = "CurrentPassword")]
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(25, ErrorMessage = "{0} must be {2} - {1} characters", MinimumLength = 8)]
        public string CurrentPassword { get; set; }

        [Display(Name = "New Password")]
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(25, ErrorMessage = "{0} must be {2} - {1} characters", MinimumLength = 8)]
        public string NewPassword { get; set; }

        [Display(Name = "Confirm New Password")]
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(25, ErrorMessage = "{0} must be {2} - {1} characters", MinimumLength = 8)]
        [Compare("NewPassword", ErrorMessage = "Password must match")]
        public string ConfirmPassword { get; set; }
    }
}
