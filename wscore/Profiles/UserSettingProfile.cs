using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Warshti.Entities.Identity;
using WScore.Entities.Identity;
using WScore.Models;

namespace WScore.Profiles
{
    public class UserSettingProfile : Profile
    {
        public UserSettingProfile()
        {
            CreateMap<UserSetting, UserSettingModel>()
                .ForMember(dest => dest.User, src => src.MapFrom(t => t.User.Name))
                .ForMember(dest => dest.Language, src => src.MapFrom(t => t.Language.Name));
        }
    }
}
