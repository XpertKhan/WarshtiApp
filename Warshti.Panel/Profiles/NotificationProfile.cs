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
    public class NotificationProfile : Profile
    {
        public NotificationProfile()
        {
            CreateMap<Notification, NotificationModel>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(x => x.User.Name))
                .ForMember(dest => dest.Time, opt => opt.MapFrom(x => x.NotificationTime.ToString("dd MMM yyyy HH:mm:ss")));
        }
    }
}
