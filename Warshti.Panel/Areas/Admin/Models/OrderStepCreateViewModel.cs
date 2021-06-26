using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Warshti.Panel.Areas.Admin.Models
{
    public class OrderStepCreateViewModel
    {
        [Required]
        public int OrderId { get; set; }
        
        [Required]
        public DateTime ActionDate { get; set; }
        
        [Required]
        public string Title { get; set; }
        
        [Required]
        public int OrderStepStatus { get; set; }
    }
}
