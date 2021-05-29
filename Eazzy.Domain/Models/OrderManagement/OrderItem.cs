using Eazzy.Domain.Models.MenuManagement;
using Eazzy.Shared.DomainCore;

namespace Eazzy.Domain.Models.OrderManagement
{
    public class OrderItem : Entity
    {
        public decimal Price { get; set; }

        public int MenuItemId { get; set; }

        public virtual MenuItem MenuItem { get; set; }

        public int OrderId { get; set; }

        public virtual Order Order { get; set; }
    }
}
