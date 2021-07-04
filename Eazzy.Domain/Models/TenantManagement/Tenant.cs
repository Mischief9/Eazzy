using Eazzy.Domain.Models.MenuManagement;
using Eazzy.Domain.Models.RestaurantManagement;
using Eazzy.Domain.Models.TenantManagement.Enums;
using Eazzy.Shared.DomainCore;
using System;
using System.Collections.Generic;

namespace Eazzy.Domain.Models.TenantManagement
{
    /// TODO : Merchant SecretKey for payment.
    public class Tenant : Entity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public string TimeZone { get; set; }

        public TaxType TaxType { get; set; }

        public decimal? TaxAmount { get; set; }

        public decimal? TaxPercentage { get; set; }

        public string ImageFileName { get; set; }

        public TenantStatus TenantStatus { get; set; }

        public DateTime CreateDateOnUtc { get; set; }

        public DateTime UpdatedDateTimeOnUtc { get; set; }

        public bool IsDeleted { get; set; }

        public virtual ICollection<Table> Tables { get; set; }

        public virtual ICollection<Menu> Menus { get; set; }
    }
}
