using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Warshti.Panel.Areas.Admin.Models;
using WScore.Entities.Identity;

namespace Warshti.Panel.Profiles
{
    public class WorkShopProfile : Profile
    {
        public WorkShopProfile()
        {
            CreateMap<User, WorkShopModel>()
                .ForMember(dest => dest.Created, opt => opt.MapFrom(x => x.Created.ToString("dd MMM yyyy")));
        }
    }
}
