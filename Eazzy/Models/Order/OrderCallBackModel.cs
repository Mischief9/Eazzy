using Eazzy.Domain.Models.OrderManagement.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eazzy.Models.Order
{
    public class OrderCallBackModel
    {
        public string Status { get; set; }

        public int OrderId { get; set; }

        public OrderStatus GetStatus()
        {
            switch (Status)
            {
                case "failed":
                    return OrderStatus.Rejected;

                case "success":
                    return OrderStatus.Success;

                default:
                    return OrderStatus.Failed;
            }
        }
    }
}
