using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WScore.Entities.Identity;

namespace WScore.Models
{
    public class WorkShopInfoUpdateModel
    {
        public UpdateUser user { get; set; }
        public string Sponsor { get; set; }
        public string Department { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string Facility { get; set; }
        public string CommercialRegister { get; set; }
        public string ElectonicPaymentAccount { get; set; }
        public IFormFile Photo { get; set; }

        public List<IFormFile> Pictures { get; set; }
        public WorkShopInfoUpdateModel()
        {
            Pictures = new List<IFormFile>();
        }
    }

    public class UpdateUser
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}
