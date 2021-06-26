using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Warshti.Entities
{
    public enum OrderStatus
    {
        Offer =  1,
        Accepted,
        InProgress,

        Declined,
        Cancelled,
        Completed
    }
}
