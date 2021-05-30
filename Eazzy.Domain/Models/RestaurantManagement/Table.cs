using Eazzy.Domain.Models.TenantManagement;
using Eazzy.Shared.DomainCore;
using Eazzy.Shared.DomainCore.Interface;

namespace Eazzy.Domain.Models.RestaurantManagement
{
    public class Table : Entity, IHasTenant
    {   
        public string TableNumber { get; set; }

        public bool IsFree { get; set; }

        public int TenantId { get; set; }

        public virtual Tenant Tenant { get; set; }
    }
}
