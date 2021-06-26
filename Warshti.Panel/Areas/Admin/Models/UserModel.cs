using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Warshti.Panel.Areas.Admin.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Created { get; set; }
        public bool IsActive { get; set; }
        public string Actions { get; set; }

        public List<string> Roles { get; set; }

        public UserModel()
        {
            Roles = new List<string>();
        }
    }
}
