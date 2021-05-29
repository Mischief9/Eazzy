using Eazzy.Shared.DomainCore;
using Eazzy.Shared.DomainCore.Interface;
using System;
using System.Collections.Generic;

namespace Eazzy.Domain.Models.MenuManagement
{
    public class Menu : Entity, IHasTenant
    {
        public int TenantId { get; set; }

        public string Name { get; set; }

        public DateTime CreateDateOnUtc { get; set; }

        public DateTime UpdatedDateOnUtc { get; set; }

        public virtual ICollection<MenuItem> MenuItems { get; set; }
    }
}
