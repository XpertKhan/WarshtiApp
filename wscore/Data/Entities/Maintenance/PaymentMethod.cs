using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WScore.Data.Entities.Car;
using WScore.Data.Entities.WScore;

namespace WScore.Data.Entities.Maintenance
{
    public class PaymentMethod
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Service> CarServices { get; set; }
        public virtual ICollection<UserPaymentMethod> UserPaymentMethods { get; set; }
        public PaymentMethod()
        {
            CarServices = new HashSet<Service>();
            UserPaymentMethods = new HashSet<UserPaymentMethod>();
        }
    }
}