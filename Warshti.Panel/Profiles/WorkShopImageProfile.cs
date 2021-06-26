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
    public class WorkShopImageProfile : Profile
    {
        public WorkShopImageProfile()
        {
            CreateMap<WorkShopImage, WorkShopImageModel>()
                .ForMember(dest => dest.WorkShop, opt => opt.MapFrom(x => x.WorkShop.Name));
        }
    }
}
