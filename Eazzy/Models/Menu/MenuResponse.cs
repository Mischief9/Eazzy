using Eazzy.Domain.Models.MenuManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eazzy.Models.Menu
{
    public class MenuResponse
    {
        public int MenuId { get; set; }

        public int TenantId { get; set; }

        public string ImageUrl { get; set; }

        public string Name { get; set; }

        public DateTime CreateDateOnUtc { get; set; }

        public DateTime UpdatedDateOnUtc { get; set; }

        public virtual IList<MenuItem> MenuItems { get; set; }
    }
}
