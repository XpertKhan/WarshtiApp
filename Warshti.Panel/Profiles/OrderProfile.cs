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
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderModel>()
                .ForMember(dest => dest.CompletionDate, opt => opt.MapFrom(x =>(x.CompletionDate.HasValue == true) ? x.CompletionDate.Value.ToString("dd MMM yyyy") : ""))
                .ForMember(dest => dest.CreationDate, opt => opt.MapFrom(x => x.CreationDate.ToString("dd MMM yyyy")))
                .ForMember(dest => dest.ExpectedCompletionDate, opt => opt.MapFrom(x => x.ExpectedCompletionDate.ToString("dd MMM yyyy")))
                .ForMember(dest => dest.Service, opt => opt.MapFrom(x => x.Service.Description))
                .ForMember(dest => dest.Workshop, opt => opt.MapFrom(x => x.Workshop.Name));
        }
    }
}
