using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WScore.Models
{
    public class AnnouncementModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Detail { get; set; }
        public int UserId { get; set; }
        public IEnumerable<AnnouncementImageModel> AnnouncementImages { get; set; }

    }
}
