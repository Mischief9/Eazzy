using Eazzy.Application.Services.CustomerService;
using Eazzy.Domain.Models.CustomerManagement;
using Eazzy.Shared.DomainCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Eazzy.Infrastructure
{
    [ApiController]
    public class WebApiController : ControllerBase
    {
        public WebApiController()
        {
        }

        protected virtual IActionResult Fail(HttpStatusCode httpStatusCode, string error = null)
        {
            var failedResponse = new FailedResponse();

            if(string.IsNullOrEmpty(error))
            {
                return BadRequest();
            }

            failedResponse.Errors.Add(error);

            return StatusCode((int)httpStatusCode, failedResponse);
        }

        protected virtual Customer GetCurrentCustomer()
        {
            var _customerService = (ICustomerService)HttpContext.RequestServices.GetService(typeof(ICustomerService));

            var username = User.Identity.Name;
            var customer = _customerService.FindByUserName(username);

            return customer;
        }
    }
}
