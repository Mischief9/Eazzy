using Eazzy.Shared.DomainCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eazzy.Application.Models.Order
{
    public class GetOrdersRequest : SortAndPaged
    {
        public int TenantId { get; set; }

        public int CustomerId { get; set; }
    }
}
