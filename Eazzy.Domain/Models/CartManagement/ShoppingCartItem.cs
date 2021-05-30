using Eazzy.Domain.Models.MenuManagement;
using Eazzy.Shared.DomainCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eazzy.Domain.Models.CartManagement
{
    public class ShoppingCartItem : Entity
    {
        public decimal Price { get; set; }

        public int MenuItemId { get; set; }

        public virtual MenuItem MenuItem { get; set; }

        public int CustomerId { get; set; }
    }
}
