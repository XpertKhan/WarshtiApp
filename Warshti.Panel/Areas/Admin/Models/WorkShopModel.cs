using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Warshti.Panel.Areas.Admin.Models
{
    public class WorkShopModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Created { get; set; }
        public bool IsActive { get; set; }
        public string Actions { get; set; }

        public List<string> Roles { get; set; }
        public WorkShopInfoModel WorkShopInfo { get; set; }
        public List<WorkShopImageModel> WorkShopImages { get; set; }

        public WorkShopModel()
        {
            Roles = new List<string>();
            WorkShopImages = new List<WorkShopImageModel>();
        }
    }
}
