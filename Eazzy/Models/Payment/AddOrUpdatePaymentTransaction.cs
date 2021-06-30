using Eazzy.Domain.Models.PaymentManagement.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eazzy.Models.Payment
{
    public class AddOrUpdatePaymentTransaction
    {
        public int Id { get; set; }

        public string ExternalTransactionIdentifier { get; set; }

        public PaymentStatus Status { get; set; }

        public TransactionType Type { get; set; }

        public string StatusCode { get; set; }

        public string StatusDescription { get; set; }

        public int? CardId { get; set; }

        public string RawRequest { get; set; }

        public string RawResponse { get; set; }
    }
}
