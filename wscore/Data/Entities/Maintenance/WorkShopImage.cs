using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WScore.Entities.Identity;

namespace WScore.Data.Entities.Maintenance
{
    public class WorkShopImage
    {
        public int Id { get; set; }
        public string ImagePath { get; set; }
        public int WorkShopId { get; set; }
        public virtual User WorkShop { get; set; }
    }
}
