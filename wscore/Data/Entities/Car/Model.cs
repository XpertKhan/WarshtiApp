using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WScore.Data.Entities.Maintenance;

namespace WScore.Data.Entities.Car
{
    public class Model
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Service> CarServices { get; set; }
        public Model()
        {
            CarServices = new HashSet<Service>();
        }
    }
}