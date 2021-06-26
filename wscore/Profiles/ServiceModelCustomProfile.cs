using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Warshti.Entities.Maintenance;
using WScore.Models;

namespace WScore.Profiles 
{
    public class ServiceModelCustomProfile : Profile
    {
        public ServiceModelCustomProfile() 
        {
            CreateMap<Service, ServiceModelCustom>();
           
        }
        
    }
}
