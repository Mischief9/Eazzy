using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Eazzy.Shared.DomainCore
{
    public class ResultSuccess
    {
        public bool Success { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        public string Error { get; set; }
    }
}
