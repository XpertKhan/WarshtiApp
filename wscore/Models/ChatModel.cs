using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WScore.Models
{
    public class ChatModel
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public string Sender { get; set; }
        public int ReceiverId { get; set; }
        public string Receiver { get; set; }
        public string Message { get; set; }
        public DateTime MessageTime { get; set; }
    }
}
