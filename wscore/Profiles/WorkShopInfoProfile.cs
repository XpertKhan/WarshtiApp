using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Warshti.Entities.Maintenance;
using WScore.Models;

namespace WScore.Profiles
{
    public class WorkShopInfoProfile : Profile
    {
        public WorkShopInfoProfile()
        {
            CreateMap<WorkShopInfo, WorkShopInfoModel>()
                .ForMember(dest => dest.Workshop, src => src.MapFrom(t => t.Workshop.Name));
            CreateMap<WorkShopImage, WorkShopImageModel>()
                .ForMember(dest => dest.Photo, src => src.MapFrom(t => t.Photo));
        }
    }
}
