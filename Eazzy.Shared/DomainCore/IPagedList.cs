using System;
using System.Collections.Generic;
using System.Text;

namespace Eazzy.Shared.DomainCore
{
    public interface IPagedList<T> : IEnumerable<T> where T : class
    {
        public int TotalCount { get; set; }

        public IList<T> Data { get; set; }
    }
}
