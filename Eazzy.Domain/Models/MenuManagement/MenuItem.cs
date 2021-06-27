using Eazzy.Domain.Models.MenuManagement.Enum;
using Eazzy.Shared.DomainCore;

namespace Eazzy.Domain.Models.MenuManagement
{
    public class MenuItem : Entity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int MenuId { get; set; }

        public virtual Menu Menu { get; set; }

        public int MenuItemTypeId { get; set; }

        public virtual MenuItemType MenuItemType { get; set; }
    }
}
