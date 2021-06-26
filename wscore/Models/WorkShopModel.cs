using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WScore.Entities.Identity;

namespace WScore.Models
{
    public class WorkShopModel
    {
        public WorkShopInfoModel WorkShopInfo { get; set; }
        public List<byte[]> Pictures { get; set; }
        public User user { get; set; }

        public WorkShopModel()
        {
            Pictures = new List<byte[]>();
        }
    }
}
