using Eazzy.Domain.Models.TenantManagement.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eazzy.Models.Restaurant
{
    public class UpdateRestaurantModel
    {
        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public string TimeZone { get; set; }

        public TaxType TaxType { get; set; }

        public decimal? TaxAmount { get; set; }

        public decimal? TaxPercentage { get; set; }

        public string Description { get; set; }

        public IFormFile Image { get; set; }
    }
}
