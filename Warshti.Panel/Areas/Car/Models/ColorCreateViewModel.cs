using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Warshti.Panel.Areas.Car.Models
{
    public class ColorCreateViewModel
    {
        [Required]
        public string Name { get; set; }
    }
}
