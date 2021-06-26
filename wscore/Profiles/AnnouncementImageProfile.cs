using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Warshti.Entities.WScore;
using WScore.Entities.Identity;
using WScore.Models;

namespace WScore.Profiles
{
    public class AnnouncementImageProfile : Profile
    {
        public AnnouncementImageProfile()
        {
            CreateMap<AnnouncementImage, AnnouncementImageModel>();
        }
    }
}
