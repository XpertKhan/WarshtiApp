using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Warshti.Entities.Maintenance;
using WScore.Entities.Identity;

namespace Warshti.Entities.WScore
{
    public class Announcement
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Detail { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }

        [ForeignKey("WorkShopInfo")]
        public int? WorkshopId { get; set; }
        public WorkShopInfo WorkShopInfo { get; set; }
        public virtual ICollection<AnnouncementImage> AnnouncementImages { get; set; }
       
    }
}
