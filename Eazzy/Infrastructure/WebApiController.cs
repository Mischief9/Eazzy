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
    }
}
