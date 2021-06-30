using Eazzy.Domain.Models.PaymentManagement;
using Eazzy.Shared.DomainCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Eazzy.Application.Services.PaymentService
{
    public interface IPaymentService
    {
        PaymentTransaction FindById(int id);

        PaymentTransaction InsertPaymentTransaction(PaymentTransaction paymentTransaction);

        void InsertPaymentTransaction(IEnumerable<PaymentTransaction> paymentTransactions);

        void UpdatePaymentTransaction(PaymentTransaction paymentTransaction);

        void DeletePaymentTransaction(PaymentTransaction paymentTransaction);

        IPagedList<PaymentTransaction> GetPaymentTransactions(SortAndPaged sortAndPaged);
    }
}
