using Eazzy.Application.Models.Order;
using Eazzy.Domain.Models.OrderManagement;
using Eazzy.Shared.DomainCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Eazzy.Application.Services.OrderService
{
    public interface IOrderService
    {
        Order FindById(int id);

        void InsertOrder(Order order);

        void InsertOrder(IEnumerable<Order> orders);

        void UpdateOrder(Order order);

        void DeleteOrder(Order order);

        IPagedList<GetOrdersResponse> GetOrders(GetOrdersRequest request);
    }
}
