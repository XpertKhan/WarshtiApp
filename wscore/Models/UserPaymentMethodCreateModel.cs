using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WScore.Models
{
    public class UserPaymentMethodCreateModel
    {
        public int PaymentMethodId { get; set; }
        public string CardHolderName { get; set; }
        public string CardNumber { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int Cvc { get; set; }
        public bool IsPreferred { get; set; }
    }
}
