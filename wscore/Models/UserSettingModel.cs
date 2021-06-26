using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WScore.Models
{
    public class UserSettingModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string User { get; set; }
        public bool EmailNotification { get; set; }
        public bool AcceptRequest { get; set; }
        public bool DeclineRequest { get; set; }

        public int LanguageId { get; set; }
        public string Language { get; set; }
    }
}
