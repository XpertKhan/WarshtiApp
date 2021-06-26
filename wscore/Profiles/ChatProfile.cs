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
    public class ChatProfile : Profile
    {
        public ChatProfile()
        {
            CreateMap<Chat, ChatModel>()
                .ForMember(dest => dest.Sender, src => src.MapFrom(t => t.Sender.Name))
                .ForMember(dest => dest.Receiver, src => src.MapFrom(t => t.Receiver.Name));
        }
    }
}
