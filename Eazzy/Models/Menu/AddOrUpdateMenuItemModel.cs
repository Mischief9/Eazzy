using Eazzy.Domain.Models.MenuManagement.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eazzy.Models.Menu
{
    public class AddOrUpdateMenuItemModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public MenuItemType MenuItemType { get; set; }

        public decimal Price { get; set; }

        public int MenuId { get; set; }
    }
}
