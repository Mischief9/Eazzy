using Eazzy.Application.Models.Account;
using Eazzy.Application.Services.AccountService;
using Eazzy.Infrastructure;
using Eazzy.Models.Account;
using Eazzy.Shared.DomainCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Eazzy.V1.Controllers
{
    [Route("v1/account")]
    public class AccountController : WebApiController
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("signup")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SignUp([FromBody] SignUpModel model)
        {
            var signUp = new SignUpRequest()
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                UserName = model.UserName,
                Password = model.Password
            };

            var result = await _accountService.Register(signUp);

            if (result.Success)
            {
                return StatusCode(StatusCodes.Status201Created);
            }

            return Fail(result.StatusCode, result.Error);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(LoginCredentialsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            var login = new LoginRequest()
            {
                Email = loginModel.Email,
                Password = loginModel.Password,
                RememberMe = loginModel.RememberMe
            };

            var result = await _accountService.Login(login);

            if (!result.Success)
            {
                return Fail(result.StatusCode, result.Error);
            }

            return Ok(result);
        }

        [HttpPost("forgetpassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ForgetPassword()
        {
            return Ok();
        }
    }
}
