using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Warshti.Entities.WScore;
using Warshti.Panel.Areas.Admin.Models;
using WScore.Entities.Identity;

namespace Warshti.Panel.Profiles
{
    public class AnnouncementImageProfile : Profile
    {
        public AnnouncementImageProfile()
        {
            CreateMap<AnnouncementImage, AnnouncementImageModel>();
        }
    }
}
