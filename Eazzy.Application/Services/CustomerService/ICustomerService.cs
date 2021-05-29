using Eazzy.Domain.Models.CustomerManagement;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Eazzy.Application.Services.CustomerService
{
    public interface ICustomerService
    {
        Customer FindById(int id);

        void InsertCustomer(Customer customer);

        void InsertCustomer(IEnumerable<Customer> customers);

        void UpdateCustomer(Customer customer);

        void DeleteCustomer(Customer customer);
    }
}
