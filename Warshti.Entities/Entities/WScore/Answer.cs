using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WScore.Entities.Identity;

namespace Warshti.Entities.WScore
{
    public class Answer
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string AnswerText { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public virtual Question Question { get; set; }
    }
}
