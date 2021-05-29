using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eazzy.Shared.DomainCore
{
    public class PagedList<T> : IPagedList<T> where T : class
    {

        public PagedList(IEnumerable<T> source, int page, int pageSize)
        {
            TotalCount = source.Count();
            Data = source.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }

        public int TotalCount { get; set; }
        public IList<T> Data { get; set; }

        public IEnumerator<T> GetEnumerator()
        {
            return Data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Data.GetEnumerator();
        }
    }
}
