using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Warshti.Entities.Car;
using WScore.Models;

namespace WScore.Profiles
{
    public class TransmissionProfile : Profile
    {
        public TransmissionProfile()
        {
            CreateMap<Transmission, TransmissionModel>();
        }
    }
}
