using Eazzy.Shared.DomainCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eazzy.Application.Models.Customer
{
    public class GetCardsRequest : SortAndPaged
    {
        public int CustomerId { get; set; }
    }
}
