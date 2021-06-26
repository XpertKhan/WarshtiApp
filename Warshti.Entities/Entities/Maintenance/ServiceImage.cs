using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Warshti.Entities.Maintenance;

namespace Warshti.Entities.Entities.Maintenance
{
    public class ServiceImage
    {
        public int Id { get; set; }
        public byte[] Photo { get; set; }
        public int ServiceId { get; set; }
        public virtual Service Service { get; set; }
    }
}
