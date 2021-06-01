using Eazzy.Application.Services.CustomerService;
using Eazzy.Application.Services.OrderService;
using Eazzy.Domain.Models.OrderManagement;
using Eazzy.Infrastructure;
using Eazzy.Shared.DomainCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Eazzy.Application.Models.Order;
using System.Net;

namespace Eazzy.V1.Controllers
{
    [Route("v1/order")]
    public class OrderController : WebApiController
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedList<GetOrdersResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status404NotFound)]
        public IActionResult GetOrders([FromQuery] SortAndPaged sortAndPaged)
        {
            var customer = GetCurrentCustomer();
            var tenantId = customer.User.TenantId;

            if (!tenantId.HasValue)
            {
                return Fail(HttpStatusCode.NotFound, "Tenant wasn't found.");
            }

            var model = _orderService.GetOrders(new GetOrdersRequest()
            {
                TenantId = tenantId.Value,
                PageIndex = sortAndPaged.PageIndex,
                PageSize = sortAndPaged.PageSize,
                Sort= sortAndPaged.Sort,
                SortBy = sortAndPaged.SortBy
            });

            return Ok(model);
        }


        [HttpPost]
        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status400BadRequest)]
        public IActionResult PlaceOrder([FromQuery] int tenantId, [FromQuery] int tableId)
        {
            var customer = GetCurrentCustomer();

            var placedOrder = _orderService.PlaceOrder(customer, tenantId, tableId);
            return Ok(placedOrder);
        }
    }
}
