using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Warshti.Entities.Car;

namespace Warshti.Entities.Maintenance
{
    public class ServiceFault
    {
        public int Id { get; set; }
        public int FaultId { get; set; }
        public virtual Fault Fault { get; set; }
        public int ServiceId { get; set; }
        public virtual Service Service { get; set; }
        
    }
}
