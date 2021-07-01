using Eazzy.Shared.DomainCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eazzy.Models.Menu
{
    public class GetMenuItemsRequest : SortAndPaged
    {
        public int[] MenuItemTypeIds { get; set; }
    }
}
