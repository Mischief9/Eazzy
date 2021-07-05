using Eazzy.Domain.Models.CustomerManagement;
using Eazzy.Domain.Models.OrderManagement.Enums;
using Eazzy.Domain.Models.TenantManagement;
using Eazzy.Shared.DomainCore;
using System;
using System.Collections.Generic;

namespace Eazzy.Domain.Models.OrderManagement
{
    public class Order : Entity
    {
        public decimal OrderTotal { get; set; }

        public decimal OrderTotalWithoutTax { get; set; }

        public decimal TaxService { get; set; }

        public DateTime CreatedOnUtc { get; set; }

        public int CustomerId { get; set; }

        public virtual Customer Customer { get; set; }

        public int TableId { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public int TenantId { get; set; }

        public virtual Tenant Tenant { get; set; }
    }
}
