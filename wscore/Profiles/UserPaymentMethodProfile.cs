using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Warshti.Entities.WScore;
using WScore.Entities.Identity;
using WScore.Models;

namespace WScore.Profiles
{
    public class UserPaymentMethodProfile : Profile
    {
        public UserPaymentMethodProfile()
        {
            CreateMap<UserPaymentMethod, UserPaymentMethodModel>()
                .ForMember(dest => dest.User, src => src.MapFrom(t => t.User.Name))
                .ForMember(dest => dest.PaymentMethod, src => src.MapFrom(t => t.PaymentMethod.Name));
        }
    }
}
