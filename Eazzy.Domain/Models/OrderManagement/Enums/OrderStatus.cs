using System;
using System.Collections.Generic;
using System.Text;

namespace Eazzy.Domain.Models.OrderManagement.Enums
{

    [Flags]
    public enum OrderStatus
    {
        Pending = 1 << 0,
        Success = 1 << 1,
        Rejected = 1 << 2
    }
}
