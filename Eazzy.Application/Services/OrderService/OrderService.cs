using Eazzy.Application.Models.Order;
using Eazzy.Application.Services.ImageService;
using Eazzy.Application.Services.RestaurantService;
using Eazzy.Application.Services.ShoppingCartService;
using Eazzy.Domain.Models.CustomerManagement;
using Eazzy.Domain.Models.OrderManagement;
using Eazzy.Domain.Models.OrderManagement.Enums;
using Eazzy.Domain.Models.TenantManagement;
using Eazzy.Domain.Models.TenantManagement.Enums;
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
        private readonly IRestaurantService _restaurantService;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IImageService _imageService;
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<Tenant> _tenantRepository;
        public OrderService(IRepository<Order> orderRepository,
            IRepository<Tenant> tenantRepository,
            IRestaurantService restaurantService,
            IShoppingCartService shoppingCartService,
            IImageService imageService)
        {
            _orderRepository = orderRepository;
            _tenantRepository = tenantRepository;
            _restaurantService = restaurantService;
            _shoppingCartService = shoppingCartService;
            _imageService = imageService;
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

            var response = orders
                .Where(x => x.TenantId == request.TenantId)
                .Select(x => new GetOrdersResponse()
                {
                    CustomerId = x.CustomerId,
                    OrderTotal = x.OrderTotal,
                    TaxService = x.TaxService,
                    CustomerName = x.Customer.GetFullName(),
                    Items = x.OrderItems.Select(x => new GetOrderItem()
                    {
                        MenuItemId = x.MenuItemId,
                        OrderId = x.OrderId,
                        Price = x.Price,
                        Name = x.MenuItem.Name
                    }).ToList()
                });

            return new PagedList<GetOrdersResponse>(response, request.PageIndex, request.PageSize);
        }

        public Order PlaceOrder(Customer customer, int tenantId, int tableId)
        {
            if (customer.ShoppingCartItems == null)
                throw new ArgumentNullException(nameof(customer.ShoppingCartItems));

            var shoppingCartItems = customer.ShoppingCartItems.ToList();
            var restaurant = _tenantRepository.Find(tenantId);
            var orderTotal = shoppingCartItems.Sum(x => x.Price);
            var tax = decimal.Zero;

            switch (restaurant.TaxType)
            {
                case TaxType.AMOUNT:
                    tax = orderTotal + restaurant.TaxAmount.Value;
                    break;
                case TaxType.PERCENTAGE:
                    tax = orderTotal * restaurant.TaxPercentage.Value / 100;
                    break;
            }

            orderTotal += tax;

            var orderItems = shoppingCartItems.Select(x => new OrderItem()
            {
                MenuItemId = x.MenuItemId,
                Price = x.Price
            }).ToList();

            var order = new Order()
            {
                CreatedOnUtc = DateTime.UtcNow,
                CustomerId = customer.Id,
                OrderItems = orderItems,
                TaxService = tax,
                OrderTotalWithoutTax = orderTotal - tax,
                OrderTotal = orderTotal,
                TableId = tableId,
                OrderStatus = OrderStatus.Pending,
                TenantId = tenantId
            };

            _orderRepository.Add(order);
            _restaurantService.SetTableLocked(tableId);

            _shoppingCartService.ClearCart(customer.Id);

            return order;
        }

        public void ChangeOrderStatus(OrderStatus status, Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            order.OrderStatus = status;

            _orderRepository.Update(order);
        }

        public IPagedList<GetOrdersResponse> GetCustomerOrders(GetOrdersRequest request)
        {
            var orders = _orderRepository.Table.AsQueryable();

            var response = orders
                .Where(x => x.CustomerId == request.CustomerId)
                .Select(x => new GetOrdersResponse()
                {
                    CustomerId = x.CustomerId,
                    OrderTotal = x.OrderTotal,
                    TaxService = x.TaxService,
                    CustomerName = x.Customer.GetFullName(),
                    ImageUrl = x.Tenant.ImageFileName,
                    Items = x.OrderItems.Select(x => new GetOrderItem()
                    {
                        MenuItemId = x.MenuItemId,
                        OrderId = x.OrderId,
                        Price = x.Price,
                        Name = x.MenuItem.Name
                    }).ToList()
                });

            return new PagedList<GetOrdersResponse>(response, request.PageIndex, request.PageSize);
        }
    }
}
