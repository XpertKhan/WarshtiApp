using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Warshti.Panel.Areas.Admin.Models
{
    public class OrderProgressViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int Progress { get; set; }

        [Required]
        public int OrderStatusId { get; set; }
    }
}
