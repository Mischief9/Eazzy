using System;
using System.Collections.Generic;
using System.Text;

namespace Eazzy.Application.Models.Order
{
    public class GetOrdersResponse
    {
        public decimal OrderTotal { get; set; }

        public decimal TaxService { get; set; }

        public string CustomerName { get; set; }
    }
}
