using Eazzy.Application.Models.Order;
using Eazzy.Application.Services.CustomerService;
using Eazzy.Application.Services.ImageService;
using Eazzy.Application.Services.OrderService;
using Eazzy.Application.Services.RoleService;
using Eazzy.Domain.Models.CustomerManagement;
using Eazzy.Infrastructure;
using Eazzy.Models.Customers;
using Eazzy.Shared.DomainCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Eazzy.V1.Controllers
{
    [Route("v1/customer")]
    [Authorize]
    public class CustomerController : WebApiController
    {
        private readonly ICustomerService _customerService;
        private readonly IOrderService _orderService;
        private readonly IRoleService _roleService;
        private readonly IImageService _imageService;

        public CustomerController(ICustomerService customerService,
            IOrderService orderService,
            IRoleService roleService,
            IImageService imageService)
        {
            _customerService = customerService;
            _orderService = orderService;
            _roleService = roleService;
            _imageService = imageService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(CustomerModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status404NotFound)]
        public IActionResult GetCustomer()
        {
            var customer = GetCurrentCustomer();
            var role = _roleService.FindByUserId(customer.User.Id);

            if (customer == null)
            {
                return Fail(HttpStatusCode.NotFound, "Customer wasn't found.");
            }

            var customerModel = new CustomerModel()
            {
                Id = customer.Id,
                UserId = customer.User.Id,
                ShoppingCartItems = customer.ShoppingCartItems.ToList(),
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                PhoneNumber = customer.PhoneNumber,
                Role = role
            };

            return Ok(customerModel);
        }

        [HttpPatch]
        [ProducesResponseType(typeof(Customer), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status404NotFound)]
        public IActionResult ChangeCustomer([FromBody] UpdateCustomerModel model)
        {
            var customer = GetCurrentCustomer();

            if (customer == null)
            {
                return Fail(HttpStatusCode.NotFound, "Customer wasn't found.");
            }

            customer.FirstName = model.FirstName;
            customer.LastName = model.LastName;
            customer.PhoneNumber = model.PhoneNumber;

            _customerService.UpdateCustomer(customer);

            return Ok(customer);
        }

        [HttpGet("order/history")]
        [ProducesResponseType(typeof(PagedList<GetOrdersResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status404NotFound)]
        public IActionResult GetCustomerOrderHistory([FromQuery] SortAndPaged sortAndPaged)
        {
            var customer = GetCurrentCustomer();

            if (customer == null)
            {
                return Fail(HttpStatusCode.NotFound, "Customer wasn't found.");
            }

            var response = _orderService.GetCustomerOrders(new GetOrdersRequest()
            {
                CustomerId = customer.Id,
                PageSize = sortAndPaged.PageSize,
                PageIndex = sortAndPaged.PageIndex,
                Sort = sortAndPaged.Sort,
                SortBy = sortAndPaged.SortBy
            });

            var model = response.Select(x => new GetOrdersResponse()
            {
                CustomerId = x.CustomerId,
                TaxService = x.TaxService,
                CustomerName = x.CustomerName,
                ImageUrl = _imageService.GetImageUrlByName(x.ImageUrl),
                Items = x.Items,
                OrderTotal = x.OrderTotal
            }).ToList();

            return Ok(new { data = model, totalCount = response.TotalCount });
        }
    }
}
