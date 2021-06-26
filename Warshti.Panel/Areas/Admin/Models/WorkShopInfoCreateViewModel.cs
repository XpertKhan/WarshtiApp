using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Warshti.Panel.Areas.Admin.Models
{
    public class WorkShopInfoCreateViewModel
    {
        public string Sponsor { get; set; }
        public string Department { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string Facility { get; set; }
        public string CommercialRegister { get; set; }
        public string ElectonicPaymentAccount { get; set; }
    }
}
