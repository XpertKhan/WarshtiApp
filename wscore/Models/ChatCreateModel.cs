using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WScore.Models
{
    public class ChatCreateModel
    {
        public int ReceiverId { get; set; }
        public string Message { get; set; }
    }
}
