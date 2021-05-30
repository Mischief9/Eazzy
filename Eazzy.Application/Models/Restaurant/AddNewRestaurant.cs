using Eazzy.Application.Models.Account;
using Eazzy.Domain.Models.TenantManagement.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eazzy.Application.Models.Restaurant
{
    public class AddNewRestaurant
    {
        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public TaxType TaxType { get; set; }

        public decimal? TaxAmount { get; set; }

        public decimal? TaxPercentage { get; set; }

        public TenantStatus TenantStatus { get; set; }

        public SignUpRequest SignUpRequest { get; set; }
    }
}
