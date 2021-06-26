using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Warshti.Entities.Maintenance;
using WScore.Models;

namespace WScore.Profiles
{
    public class ServiceFaultProfile : Profile
    {
        public ServiceFaultProfile()
        {
            CreateMap<ServiceFault, ServiceFaultModel>()
                .ForMember(dest => dest.Fault, src => src.MapFrom(t => t.Fault.Name))
                .ForMember(dest => dest.Service, src => src.MapFrom(t => t.Service.Description));
        }
    }
}
