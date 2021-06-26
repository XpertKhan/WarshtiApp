using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WScore.Data.Entities.WScore
{
    public class AnnouncementImage
    {
        public int Id { get; set; }
        public string ImagePath { get; set; }
        public int AnnouncementId { get; set; }
        public virtual Announcement Announcement { get; set; }
    }
}
