using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Warshti.Panel.Areas.Car.Models
{
    public class ModelUpdateViewModel
    {
        [Required]
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
    }
}
