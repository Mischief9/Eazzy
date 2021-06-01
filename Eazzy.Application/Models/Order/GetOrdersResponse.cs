using System;
using System.Collections.Generic;
using System.Text;

namespace Eazzy.Application.Models.Order
{
    public class GetOrdersResponse
    {
        public int CustomerId { get; set; }

        public decimal OrderTotal { get; set; }

        public decimal TaxService { get; set; }

        public string CustomerName { get; set; }

        public List<GetOrderItem> Items { get; set; }
    }

    public class GetOrderItem
    {
        public int MenuItemId { get; set; }

        public decimal Price { get; set; }

        public int OrderId { get; set; }

        public string Name { get; set; }
    }
}
