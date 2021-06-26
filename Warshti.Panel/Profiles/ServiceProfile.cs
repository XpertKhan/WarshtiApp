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
    public class ServiceProfile : Profile
    {
        public ServiceProfile()
        {
            CreateMap<Service, ServiceModel>()
                .ForMember(dest => dest.Color, opt => opt.MapFrom(x => x.Color.Name))
                .ForMember(dest => dest.Company, opt => opt.MapFrom(x => x.Company.Name))
                .ForMember(dest => dest.Department, opt => opt.MapFrom(x => x.Department.Name))
                .ForMember(dest => dest.Model, opt => opt.MapFrom(x => x.Model.Name))
                .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(x => x.PaymentMethod.Name))
                .ForMember(dest => dest.Transmission, opt => opt.MapFrom(x => x.Transmission.Name))
                .ForMember(dest => dest.User, opt => opt.MapFrom(x => x.User.Name));
        }
    }
}
