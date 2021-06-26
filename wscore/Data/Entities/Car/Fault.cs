using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WScore.Data.Entities.Maintenance;

namespace WScore.Data.Entities.Car
{
    public class Fault
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ServiceFault> ServiceFaults { get; set; }
        public Fault()
        {
            ServiceFaults = new HashSet<ServiceFault>();
        }
    }
}
