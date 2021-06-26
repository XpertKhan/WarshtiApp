using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Warshti.Panel.Areas.Admin.Models
{
    public class NotificationUpdateViewModel
    {
        [Required]
        public int Id { get; set; }
        
        [Required]
        public int UserId { get; set; }
        public IEnumerable<SelectListItem> Users { get; set; }

        [Required]
        public string Message { get; set; }
    }
}
