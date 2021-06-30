using Eazzy.Application.Services.CustomerService;
using Eazzy.Application.Services.OrderService;
using Eazzy.Domain.Models.OrderManagement;
using Eazzy.Infrastructure;
using Eazzy.Shared.DomainCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Eazzy.Application.Models.Order;
using System.Net;
using Eazzy.Models.Order;
using Eazzy.Models.Payment;
using Eazzy.Application.Services.PaymentService;
using Eazzy.Domain.Models.PaymentManagement;
using System.Threading.Tasks;
using System;

namespace Eazzy.V1.Controllers
{
    [Route("v1/order")]
    public class OrderController : WebApiController
    {
        private readonly IOrderService _orderService;
        private readonly IPaymentService _paymentService;

        public OrderController(IOrderService orderService,
            IPaymentService paymentService)
        {
            _orderService = orderService;
            _paymentService = paymentService;
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
                Sort = sortAndPaged.Sort,
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

        [HttpPost("callback")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status400BadRequest)]
        public IActionResult CallBackOrderStatus([FromBody] OrderCallBackModel model)
        {
            var order = _orderService.FindById(model.OrderId);

            if (order == null)
            {
                return Fail(HttpStatusCode.NotFound, "Order wasn't found.");
            }

            var status = model.GetStatus();
            _orderService.ChangeOrderStatus(status, order);

            return NoContent();
        }

        [HttpGet("paymenttransaction")]
        [ProducesResponseType(typeof(PagedList<PaymentTransaction>),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status400BadRequest)]
        public IActionResult GetPaymentTransactions([FromQuery]SortAndPaged sortAndPaged)
        {
            var paymentTransactions = _paymentService.GetPaymentTransactions(sortAndPaged);

            return Ok(paymentTransactions);
        }

        [HttpPost("paymenttransaction")]
        [ProducesResponseType(typeof(PaymentTransaction), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status400BadRequest)]
        public IActionResult AddPaymentTransaction([FromBody]AddOrUpdatePaymentTransaction model)
        {
            var newPaymentTransaction = new PaymentTransaction()
            {
                SecondaryId = Guid.NewGuid(),
                ExternalTransactionIdentifier = model.ExternalTransactionIdentifier,
                Status = model.Status,
                StatusCode = model.StatusCode,
                StatusDescription = model.StatusDescription,
                CardId = model.CardId,
                RawRequest = model.RawRequest,
                RawResponse = model.RawResponse,
                Type = model.Type,
                CreateDateOnUtc = DateTime.UtcNow
            };

            var paymentTransaction = _paymentService.InsertPaymentTransaction(newPaymentTransaction);

            return Ok(paymentTransaction);
        }

        [HttpPatch("paymenttransaction")]
        [ProducesResponseType(typeof(PaymentTransaction), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status404NotFound)]
        public IActionResult UpdatePaymentTransaction([FromBody]AddOrUpdatePaymentTransaction model)
        {
            var paymentTransaction = _paymentService.FindById(model.Id);

            if(paymentTransaction == null)
            {
                return Fail(HttpStatusCode.NotFound, "Payment Transaction wasn't found.");
            }

            paymentTransaction.ExternalTransactionIdentifier = model.ExternalTransactionIdentifier;
            paymentTransaction.Status = model.Status;
            paymentTransaction.StatusCode = model.StatusCode;
            paymentTransaction.StatusDescription = model.StatusDescription;
            paymentTransaction.CardId = model.CardId;
            paymentTransaction.RawRequest = model.RawRequest;
            paymentTransaction.RawResponse = model.RawResponse;
            paymentTransaction.Type = model.Type;
            paymentTransaction.UpdateDateOnutc = DateTime.UtcNow;

            _paymentService.UpdatePaymentTransaction(paymentTransaction);

            return Ok(paymentTransaction);
        }
    }
}
