using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Warshti.Panel.Areas.Admin.Models
{
    public class AnnouncementCreateViewModel
    {
        [Required]
        public int UserId { get; set; }
        public IEnumerable<SelectListItem> Users { get; set; }

        [Required]
        public string Title { get; set; }
        
        [Required]
        public string Description { get; set; }
        public string Detail { get; set; }
    }
}
