using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Warshti.Panel.Areas.Admin.Models
{
    public class OrderStepModel
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string ActionDate { get; set; }
        public string Title { get; set; }
        public int OrderStepStatus { get; set; }
    }
}
