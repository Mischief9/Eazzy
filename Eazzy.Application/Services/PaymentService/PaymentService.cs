using Eazzy.Domain.Models.PaymentManagement;
using Eazzy.Infrastructure.Repository.Interfaces;
using Eazzy.Shared.DomainCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eazzy.Application.Services.PaymentService
{
    public class PaymentService : IPaymentService
    {
        private readonly IRepository<PaymentTransaction> _paymentTransactionRepository;

        public PaymentService(IRepository<PaymentTransaction> paymentTransactionRepository)
        {
            _paymentTransactionRepository = paymentTransactionRepository;
        }

        public void DeletePaymentTransaction(PaymentTransaction paymentTransaction)
        {
            if (paymentTransaction == null)
                throw new ArgumentNullException(nameof(paymentTransaction));

            _paymentTransactionRepository.Delete(paymentTransaction);
        }

        public PaymentTransaction FindById(int id)
        {
            var paymentTransaction = _paymentTransactionRepository.Find(id);

            return paymentTransaction;
        }

        public IPagedList<PaymentTransaction> GetPaymentTransactions(SortAndPaged sortAndPaged)
        {
            var paymentTransactions = _paymentTransactionRepository.Table;

            return new PagedList<PaymentTransaction>(paymentTransactions, sortAndPaged.PageIndex, sortAndPaged.PageSize);
        }

        public PaymentTransaction InsertPaymentTransaction(PaymentTransaction paymentTransaction)
        {
            if (paymentTransaction == null)
                throw new ArgumentNullException(nameof(paymentTransaction));

            _paymentTransactionRepository.Add(paymentTransaction);

            return paymentTransaction;
        }

        public void InsertPaymentTransaction(IEnumerable<PaymentTransaction> paymentTransactions)
        {
            if (paymentTransactions == null)
                throw new ArgumentNullException(nameof(paymentTransactions));

            _paymentTransactionRepository.Delete(paymentTransactions);
        }

        public void UpdatePaymentTransaction(PaymentTransaction paymentTransaction)
        {
            if (paymentTransaction == null)
                throw new ArgumentNullException(nameof(paymentTransaction));

            _paymentTransactionRepository.Update(paymentTransaction);
        }
    }
}
