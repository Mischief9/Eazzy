using Eazzy.Application.Models;
using Eazzy.Application.Models.Account;
using Eazzy.Shared.DomainCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Eazzy.Application.Services.AccountService
{
    public interface IAccountService
    {
        Task<ResultSuccess> Register(SignUpRequest signUp);

        Task<LoginCredentialsResponse> Login(LoginRequest login);
    }
}
