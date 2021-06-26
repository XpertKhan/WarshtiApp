using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Warshti.Panel.Areas.Admin.Models
{
    public class UserUpdateViewModel
    {
        [Required]
        public int Id { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(250, ErrorMessage = "{0} must be {2} - {1} characters", MinimumLength = 3)]
        public string Name { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "{0} is required")]
        [EmailAddress(ErrorMessage = "{0} must be a valid email address")]
        public string Email { get; set; }

        [Display(Name = "Mobile")]
        [StringLength(25, ErrorMessage = "{0} may have max. {1} characters")]
        [Required]
        public string Mobile { get; set; }

        public List<UserRoleViewModel> Roles { get; set; }

        public UserUpdateViewModel()
        {
            Roles = new List<UserRoleViewModel>();
        }
    }
}
