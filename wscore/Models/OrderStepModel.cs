using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WScore.Models
{
    public class OrderStepModel
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string Order { get; set; }
        public DateTime ActionDate { get; set; }
        public string Title { get; set; }
        public int OrderStepStatus { get; set; }
    }
}
