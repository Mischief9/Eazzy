using Eazzy.Domain.Models.RestaurantManagement;
using Eazzy.Domain.Models.TenantManagement.Enums;
using Eazzy.Domain.Models.MenuManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eazzy.Models.Restaurant
{
    public class RestaurantListResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public string TimeZone { get; set; }

        public TaxType TaxType { get; set; }

        public decimal? TaxAmount { get; set; }

        public decimal? TaxPercentage { get; set; }

        public string ImageUrl { get; set; }

        public TenantStatus TenantStatus { get; set; }

        public DateTime CreateDateOnUtc { get; set; }

        public DateTime UpdatedDateTimeOnUtc { get; set; }
    }
}
