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
    public class OrderStepProfile : Profile
    {
        public OrderStepProfile()
        {
            CreateMap<OrderStep, OrderStepModel>()
                .ForMember(dest => dest.ActionDate, opt => opt.MapFrom(x => x.ActionDate.ToString("dd MMM yyyy")));
        }
    }
}
