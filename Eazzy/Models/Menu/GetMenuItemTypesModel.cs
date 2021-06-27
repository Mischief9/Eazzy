using Eazzy.Shared.DomainCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eazzy.Models.Menu
{
    public class GetMenuItemTypesModel : SortAndPaged
    {
        public string Name { get; set; }
    }
}
