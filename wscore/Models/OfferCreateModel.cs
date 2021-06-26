using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WScore.Models
{
    public class OfferCreateModel
    {
        public int ServiceId { get; set; }
        public int WorkshopId { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExpectedCompletionDate { get; set; }
        public decimal EstimatedPrice { get; set; }

    }
}
