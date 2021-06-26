using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Warshti.Entities.WScore
{
    public class QuestionImage
    {
        public int Id { get; set; }
        public byte[] Photo { get; set; }
        public int QuestionId { get; set; }
        public virtual Question Question { get; set; }
    }
}
