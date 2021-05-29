
using System;
using System.Collections.Generic;
using System.Text;

namespace Eazzy.Application.Models.Account
{
    public class SignUpRequest
    {
        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Password { get; set; }
    }
}
