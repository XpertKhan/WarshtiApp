using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Warshti.Panel.Areas.Admin.Models
{
    public class OrderModel
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public string Service { get; set; }
        public int WorkshopId { get; set; }
        public string Workshop { get; set; }
        public string CreationDate { get; set; }
        public string ExpectedCompletionDate { get; set; }
        public string CompletionDate { get; set; }
        public string OrderNumber { get; set; }
        public decimal EstimatedPrice { get; set; }
        public int OrderStatusId { get; set; }
        public int OrderProgress { get; set; }
    }
}
