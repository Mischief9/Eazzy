using Eazzy.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eazzy.Shared.DomainCore
{
    public class SortAndPaged
    {
        public string Sort { get; set; } = "Id";

        public SortBy SortBy { get; set; } = SortBy.DESC;

        public int PageSize { get; set; } = 10;

        public int PageIndex { get; set; } = 1;
    }
}
