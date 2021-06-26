using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WScore.Models
{
    public class ServiceCreateModel
    {
        public string Description { get; set; }

        public int CompanyId { get; set; }
        public int ModelId { get; set; }
        public int ColorId { get; set; }
        public int TransmissionId { get; set; }
        public int PaymentMethodId { get; set; }
        public int DepartmentId { get; set; }

        public List<int> Faults { get; set; }
        public List<IFormFile> Photos { get; set; }

        public ServiceCreateModel()
        {
            Faults = new List<int>();
        }
    }
}
