using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Warshti.Entities.Entities
{
    public class DeviceToken
    {
        public int Id { get; set; }
        public string Device_Token { get; set; }
        public int User_Id { get; set; }
    }
}
