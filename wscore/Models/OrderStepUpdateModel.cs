using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WScore.Models
{
    public class OrderStepUpdateModel
    {
        public DateTime ActionDate { get; set; }
        public string Title { get; set; }
        public int OrderStepStatus { get; set; }
    }
}
