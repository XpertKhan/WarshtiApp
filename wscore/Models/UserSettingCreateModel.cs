using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WScore.Models
{
    public class UserSettingCreateModel
    {
        public bool EmailNotification { get; set; }
        public bool AcceptRequest { get; set; }
        public bool DeclineRequest { get; set; }
        public int LanguageId { get; set; }
    }
}
