using Eazzy.Domain.Models.CustomerManagement;
using Eazzy.Infrastructure.Repository.Interfaces;
using Eazzy.Shared.DomainCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eazzy.Application.Services.CustomerService

{
    public class CustomerService : ICustomerService
    {
        private readonly IRepository<Customer> _customerRepository;
        public CustomerService(IRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public Customer FindById(int id)
        {
            var customer = _customerRepository.Find(id);

            return customer;
        }

        public void InsertCustomer(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException();

            _customerRepository.Add(customer);
        }

        public void InsertCustomer(IEnumerable<Customer> customers)
        {
            if (customers == null)
                throw new ArgumentNullException();

            _customerRepository.Add(customers);
        }

        public void UpdateCustomer(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException();

            _customerRepository.Update(customer);
        }

        public void DeleteCustomer(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException();

            _customerRepository.Delete(customer);
        }

        public Customer FindByUserName(string username)
        {
            if (string.IsNullOrEmpty(username))
                throw new ArgumentNullException(username);

            return _customerRepository.Table.Include("User").SingleOrDefault(x => x.User.UserName == username);
        }
    }
}
