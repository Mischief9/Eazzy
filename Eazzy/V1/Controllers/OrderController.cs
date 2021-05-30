using Eazzy.Application.Services.CustomerService;
using Eazzy.Application.Services.OrderService;
using Eazzy.Domain.Models.OrderManagement;
using Eazzy.Infrastructure;
using Eazzy.Shared.DomainCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eazzy.V1.Controllers
{
    [Route("v1/order")]
    public class OrderController : WebApiController
    {
        private readonly IOrderService _orderService;
        private readonly ICustomerService _customerService;

        public OrderController(IOrderService orderService,
            ICustomerService customerService)
        {
            _orderService = orderService;
            _customerService = customerService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status400BadRequest)]
        public IActionResult PlaceOrder([FromQuery] int tenantId, [FromQuery]int tableId)
        {
            var username = User.Identity.Name;
            var customer = _customerService.FindByUserName(username);

            var placedOrder = _orderService.PlaceOrder(customer, tenantId, tableId);
            return Ok(placedOrder);
        }
    }
}
