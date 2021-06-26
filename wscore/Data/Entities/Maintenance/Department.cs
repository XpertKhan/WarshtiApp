using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WScore.Data.Entities.Car;

namespace WScore.Data.Entities.Maintenance
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Service> CarServices { get; set; }
        public Department()
        {
            CarServices = new HashSet<Service>();
        }
    }
}