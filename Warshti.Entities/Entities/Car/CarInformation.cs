using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Warshti.Entities.Entities.Car
{
    public class CarInformation
    {
        [Key]
        public int Id { get; set; }
        public int Company { get; set; }
        public int Model { get; set; }
        public int Color { get; set; }
        public string CarTransmission { get; set; }
        public int UserId { get; set; }
    }
}
