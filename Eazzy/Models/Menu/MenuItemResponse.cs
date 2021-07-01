using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eazzy.Models.Menu
{
    public class MenuItemResponse
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string ImageUrl { get; set; }

        public int MenuId { get; set; }

        public int MenuItemTypeId { get; set; }
    }
}
