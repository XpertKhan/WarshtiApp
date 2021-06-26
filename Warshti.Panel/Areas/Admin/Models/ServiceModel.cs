using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Warshti.Panel.Areas.Admin.Models
{
    public class ServiceModel
    {
        public int Id { get; set; }
        public string Description { get; set; }

        public int CompanyId { get; set; }
        public string Company { get; set; }
        public int ModelId { get; set; }
        public string Model { get; set; }
        public int ColorId { get; set; }
        public string Color { get; set; }
        public int TransmissionId { get; set; }
        public string Transmission { get; set; }
        public int PaymentMethodId { get; set; }
        public string PaymentMethod { get; set; }
        public int DepartmentId { get; set; }
        public string Department { get; set; }

        public int UserId { get; set; }
        public string User { get; set; }
        public int ServiceStatus { get; set; }

        //public virtual ICollection<ServiceFault> ServiceFaults { get; set; }
        //public virtual ICollection<Order> Orders { get; set; }
        //public Service()
        //{
        //    ServiceFaults = new HashSet<ServiceFault>();
        //    Orders = new HashSet<Order>();
        //}

        public ServiceModel()
        {

        }
    }
}
