using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eazzy.Models.Cart
{
    public class CartDetails
    {

        public decimal OrderTotal { get; set; }

        public decimal Tax { get; set; }

        public int TenantId { get; set; }

        public List<CartItem> CartItems { get; set; }
    }

    public class CartItem
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public int MenuItemId { get; set; }

        public int CustomerId { get; set; }
    }
}
