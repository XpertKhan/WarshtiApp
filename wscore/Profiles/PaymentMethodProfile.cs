using AutoMapper;
using Warshti.Entities.Maintenance;
using WScore.Models;

namespace WScore.Profiles
{
    public class PaymentMethodProfile : Profile
    {
        public PaymentMethodProfile()
        {
            CreateMap<PaymentMethod, PaymentMethodModel>();
        }
    }
}
