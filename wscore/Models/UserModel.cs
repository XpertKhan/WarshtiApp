using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WScore.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTimeOffset Created { get; set; }
        public string PhoneNumber { get; set; }
        public int UserTypeId { get; set; }
    }
}
