using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Musan.Panel.Areas.Admin.Models
{
    
    public class WorkshopInfoUpdateViewModel
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
