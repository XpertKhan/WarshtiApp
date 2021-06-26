﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Warshti.Panel.Areas.Admin.Models
{
    public class OrderUpdateViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int ServiceId { get; set; }
        public List<SelectListItem> Services { get; set; }
        
        [Required]
        public int WorkshopId { get; set; }
        public List<SelectListItem> Workshops { get; set; }
        
        [Required]
        public DateTime CreationDate { get; set; }

        [Required]
        public DateTime ExpectedCompletionDate { get; set; }

        [Required]
        public int OrderProgress { get; set; }
        
        [Required]
        public decimal EstimatedPrice { get; set; }
    }
}
