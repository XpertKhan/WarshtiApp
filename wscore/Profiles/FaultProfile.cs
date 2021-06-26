using AutoMapper;
using Warshti.Entities.Car;
using WScore.Models;

namespace WScore.Profiles
{
    public class FaultProfile : Profile
    {
        public FaultProfile()
        {
            CreateMap<Fault, FaultModel>();
        }
    }
}
