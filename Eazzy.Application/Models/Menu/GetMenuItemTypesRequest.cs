using Eazzy.Shared.DomainCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eazzy.Application.Models.Menu
{
    public class GetMenuItemTypesRequest : SortAndPaged
    {
        public string Name { get; set; }
    }
}
