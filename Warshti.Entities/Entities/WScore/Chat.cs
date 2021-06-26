using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WScore.Entities.Identity;

namespace Warshti.Entities.WScore
{
    public class Chat
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public virtual User Sender { get; set; }
        public int ReceiverId { get; set; }
        public virtual User Receiver { get; set; }
        public string Message { get; set; }
        public DateTime MessageTime { get; set; }
    }
}
