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
    public class QuestionProfile : Profile
    {
        public QuestionProfile()
        {
            CreateMap<Question, QuestionModel>();
            CreateMap<Answer, AnswerModel>();
        }
    }
}
