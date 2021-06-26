using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WScore.Data.Entities.Car;
using WScore.Entities.Identity;

namespace WScore.Data.Entities.Maintenance
{
    public class Service
    {
        public int Id { get; set; }
        public string Description { get; set; }

        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
        public int ModelId { get; set; }
        public virtual Model Model { get; set; }
        public int ColorId { get; set; }
        public virtual Color Color { get; set; }
        public int TransmissionId { get; set; }
        public virtual Transmission Transmission { get; set; }
        public int PaymentMethodId { get; set; }
        public virtual PaymentMethod PaymentMethod { get; set; }
        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }

        public virtual ICollection<ServiceFault> ServiceFaults { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public Service()
        {
            ServiceFaults = new HashSet<ServiceFault>();
            Orders = new HashSet<Order>();
        }
    }
}
