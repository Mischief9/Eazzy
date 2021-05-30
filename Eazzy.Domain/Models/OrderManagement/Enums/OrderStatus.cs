using System;
using System.Collections.Generic;
using System.Text;

namespace Eazzy.Domain.Models.OrderManagement.Enums
{

    [Flags]
    public enum OrderStatus
    {
        Pending = 1 << 0,
        Failed = 1 << 1,
        Success = 1 << 2
    }
}
