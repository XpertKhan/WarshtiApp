using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WScore.Models
{
    public class WorkShopInfoModel
    {
        public int Id { get; set; }
        public int WorkShopId { get; set; }
        public string Workshop { get; set; }
        public string Sponsor { get; set; }
        public string Department { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string Facility { get; set; }
        public string CommercialRegister { get; set; }
        public string ElectonicPaymentAccount { get; set; }
        public byte[] Photo { get; set; }
    }
}
