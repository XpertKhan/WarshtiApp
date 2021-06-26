using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Warshti.Panel.Areas.Admin.Models
{
    public class WorkShopImageModel
    {
        public int Id { get; set; }
        public int WorkShopId { get; set; }
        public string WorkShop { get; set; }
        public byte[] Photo { get; set; }
    }
}
