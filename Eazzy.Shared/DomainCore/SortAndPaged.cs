using Eazzy.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eazzy.Shared.DomainCore
{
    public class SortAndPaged
    {
        public string Sort { get; set; }

        public SortBy SortBy { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }
    }
}
