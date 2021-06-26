using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WScore.Entities.Identity;

namespace Warshti.Entities.WScore
{
    public class Notification
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public string Message { get; set; }
        public DateTime NotificationTime { get; set; }
    }
}
