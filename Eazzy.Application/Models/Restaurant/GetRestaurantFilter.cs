using Eazzy.Shared.DomainCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eazzy.Application.Models.Restaurant
{
    public class GetRestaurantFilter : SortAndPaged
    {
        public string Name { get; set; }
    }
}
