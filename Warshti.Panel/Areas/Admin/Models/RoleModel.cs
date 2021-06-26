using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Warshti.Panel.Areas.Admin.Models
{
    public class RoleModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Created { get; set; }
        public string Actions { get; set; }

        public List<string> Permissions { get; set; }

        public RoleModel()
        {
            Permissions = new List<string>();
        }
    }
}
