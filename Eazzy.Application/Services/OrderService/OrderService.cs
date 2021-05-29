using Eazzy.Application.Models.Order;
using Eazzy.Domain.Models.OrderManagement;
using Eazzy.Infrastructure.Repository.Interfaces;
using Eazzy.Shared.DomainCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eazzy.Application.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly IRepository<Order> _orderRepository;

        public OrderService(IRepository<Order> orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public Order FindById(int id)
        {
            var order = _orderRepository.Find(id);

            return order;
        }

        public void InsertOrder(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            _orderRepository.Add(order);
        }

        public void InsertOrder(IEnumerable<Order> orders)
        {
            if (orders == null)
                throw new ArgumentNullException(nameof(orders));

            _orderRepository.Add(orders);
        }

        public void UpdateOrder(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            _orderRepository.Update(order);
        }

        public void DeleteOrder(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            _orderRepository.Delete(order);
        }

        public IPagedList<GetOrdersResponse> GetOrders(GetOrdersRequest request)
        {
            var orders = _orderRepository.Table.AsQueryable();
            var response = orders.Select(x => new GetOrdersResponse()
            {
                OrderTotal = x.OrderTotal,
                TaxService = x.TaxService,
                CustomerName = x.Customer.GetFullName(),
            });

            return new PagedList<GetOrdersResponse>(response, request.PageIndex, request.PageSize);
        }
    }
}
