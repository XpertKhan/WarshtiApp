using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Warshti.Entities.WScore;
using WScore.Entities.Identity;

namespace Warshti.Entities.Identity
{
    public class UserSetting
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public bool EmailNotification { get; set; }
        public bool AcceptRequest { get; set; }
        public bool DeclineRequest { get; set; }

        public int LanguageId { get; set; }
        public virtual Language Language{ get; set; }
    }
}
