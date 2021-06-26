using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Warshti.Entities.Maintenance;
using WScore.Models;

namespace WScore.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderModel>()
                .ForMember(dest => dest.Service, src => src.MapFrom(t => t.Service.Description))
                .ForMember(dest => dest.Workshop, src => src.MapFrom(t => t.Workshop.Name));
        }
    }
}
