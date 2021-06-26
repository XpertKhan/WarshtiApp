using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Warshti.Entities.Identity;
using WScore.Entities.Identity;

namespace Warshti.Entities.WScore
{
    public class Language
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<UserSetting> UserSettings { get; set; }
        public Language()
        {
            UserSettings = new HashSet<UserSetting>();
        }
    }
}
