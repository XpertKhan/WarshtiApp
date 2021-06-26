using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WScore.Models
{
    public class OrderModel
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public string Service { get; set; }
        public int WorkshopId { get; set; }
        public string Workshop { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExpectedCompletionDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public string OrderNumber { get; set; }
        public decimal EstimatedPrice { get; set; }
        public int OrderStatusId { get; set; }
        public int OrderProgress { get; set; }

        public List<OrderStepModel> OrderSteps { get; set; }
        public OrderModel()
        {
            OrderSteps = new List<OrderStepModel>();
        }
    }
}
