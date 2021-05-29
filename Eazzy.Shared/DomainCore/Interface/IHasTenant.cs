using System;
using System.Collections.Generic;
using System.Text;

namespace Eazzy.Shared.DomainCore.Interface
{
    public interface IHasTenant
    {
        public int TenantId { get; set; }
    }
}
