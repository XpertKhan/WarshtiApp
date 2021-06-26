using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WScore.Models
{
    public class AnnouncementUpdateModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Detail { get; set; }
        public int UserId { get; set; }
        public List<IFormFile> Photos { get; set; }

        public AnnouncementUpdateModel()
        {
            Photos = new List<IFormFile>();
        }

    }
}
