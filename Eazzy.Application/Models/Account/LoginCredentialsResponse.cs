using Eazzy.Shared.DomainCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Eazzy.Application.Models.Account
{
    public class LoginCredentialsResponse : ResultSuccess
    {
        public string AccessToken { get; set; }

        public IList<Claim> UserClaims { get; set; }
    }
}
