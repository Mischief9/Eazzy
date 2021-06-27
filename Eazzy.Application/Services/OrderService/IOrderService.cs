using Eazzy.Application.Models.Order;
using Eazzy.Domain.Models.CustomerManagement;
using Eazzy.Domain.Models.OrderManagement;
using Eazzy.Domain.Models.OrderManagement.Enums;
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

        Order PlaceOrder(Customer customer, int tenantId, int tableId);

        IPagedList<GetOrdersResponse> GetOrders(GetOrdersRequest request);

        IPagedList<GetOrdersResponse> GetCustomerOrders(GetOrdersRequest request);

        void ChangeOrderStatus(OrderStatus status, Order order);
    }
}
