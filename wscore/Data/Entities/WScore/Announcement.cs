using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WScore.Entities.Identity;

namespace WScore.Data.Entities.WScore
{
    public class Announcement
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Detail { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<AnnouncementImage> AnnouncementImages { get; set; }
    }
}
