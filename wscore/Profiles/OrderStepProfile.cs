using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Warshti.Entities.Maintenance;
using WScore.Models;

namespace WScore.Profiles
{
    public class OrderStepProfile : Profile
    {
        public OrderStepProfile()
        {
            CreateMap<OrderStep, OrderStepModel>()
                .ForMember(dest => dest.Order, src => src.MapFrom(t => t.Order.OrderNumber));
        }
    }
}
