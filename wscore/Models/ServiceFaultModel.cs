using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WScore.Models
{
    public class ServiceFaultModel
    {
        public int Id { get; set; }
        public int FaultId { get; set; }
        public string Fault { get; set; }
        public int ServiceId { get; set; }
        public string Service { get; set; }
    }
}
