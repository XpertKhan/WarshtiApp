using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Warshti.Entities.Maintenance;
using WScore.Models;

namespace WScore.Profiles
{
    public class ServiceProfile : Profile
    {
        public ServiceProfile()
        {
            CreateMap<Service, ServiceModel>()
                .ForMember(dest => dest.User, src => src.MapFrom(t => t.User.Name))
                .ForMember(dest => dest.Color, src => src.MapFrom(t => t.Color.Name))
                .ForMember(dest => dest.Company, src => src.MapFrom(t => t.Company.Name))
                .ForMember(dest => dest.Department, src => src.MapFrom(t => t.Department.Name))
                .ForMember(dest => dest.Model, src => src.MapFrom(t => t.Model.Name))
                .ForMember(dest => dest.PaymentMethod, src => src.MapFrom(t => t.PaymentMethod.Name))
                .ForMember(dest => dest.Transmission, src => src.MapFrom(t => t.Transmission.Name));
        }
    }
}
