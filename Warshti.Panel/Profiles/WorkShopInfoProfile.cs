using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Warshti.Entities.Maintenance;
using Warshti.Panel.Areas.Admin.Models;
using WScore.Entities.Identity;

namespace Warshti.Panel.Profiles
{
    public class WorkShopInfoProfile : Profile
    {
        public WorkShopInfoProfile()
        {
            CreateMap<WorkShopInfo, WorkShopInfoModel>()
                .ForMember(dest => dest.Workshop, opt => opt.MapFrom(x => x.Workshop.Name));
        }
    }
}
