using Eazzy.Application.Models;
using Eazzy.Application.Models.Account;
using Eazzy.Domain.Models.AccountManagement;
using Eazzy.Domain.Models.CustomerManagement;
using Eazzy.Infrastructure.Models;
using Eazzy.Infrastructure.Repository.Interfaces;
using Eazzy.Shared.DomainCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Eazzy.Application.Services.AccountService
{
    public class AccountService : IAccountService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        private readonly EazzyDbContext _db;

        public AccountService(IConfiguration configuration,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            EazzyDbContext db,
            IRepository<Customer> customerRepository)
        {
            _configuration = configuration;
            _userManager = userManager;
            _db = db;
        }

        public async Task<ResultSuccess> Register(SignUpRequest signUp, bool isAdministrator = false)
        {
            var customer = new Customer()
            {
                FirstName = signUp.FirstName,
                LastName = signUp.LastName,
                PhoneNumber = signUp.PhoneNumber
            };

            var user = new User()
            {
                UserName = signUp.UserName,
                Email = signUp.Email,
                TenantId = signUp.TenantId,
                Customer = customer
            };


            var result = await _userManager.CreateAsync(user, signUp.Password);


            if (result.Succeeded)
            {
                if (isAdministrator)
                {
                    var administratorRole = _db.Set<Role>().First(x => x.Name == "Administrator");
                    var newUser = _db.Set<User>().First(x => x.UserName == signUp.UserName);

                    var userRole = new UserRole()
                    {
                        RoleId = administratorRole.Id,
                        UserId = newUser.Id
                    };

                    _db.Set<UserRole>().Add(userRole);
                    _db.SaveChanges();
                }

                return new ResultSuccess { Success = true };
            }

            return new ResultSuccess { Success = false, Error = string.Join(',', result.Errors.Select(x => x.Description)) };
        }

        public async Task<LoginCredentialsResponse> Login(LoginRequest login)
        {
            var user = _db.Set<User>().FirstOrDefault(x => x.Email == login.Email);

            if (user != null)
            {
                var passwordIsCorrect = await _userManager.CheckPasswordAsync(user, login.Password);

                if (passwordIsCorrect)
                {
                    var tokenClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                        new Claim("tenant_id", user.TenantId.ToString()),
                    };

                    var userClaims = _db.Set<UserClaim>().Where(x => x.UserId == user.Id).ToList();

                    if (userClaims.Any())
                    {
                        foreach (var claim in userClaims)
                        {
                            tokenClaims.Add(new Claim(claim.ClaimType.ToString(), claim.ClaimValue.ToString()));
                        }
                    }

                    var configurationSection = _configuration.GetSection("TokenValidationParameters");

                    var signInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configurationSection["key"].ToString()));

                    var token = new JwtSecurityToken(
                        issuer: configurationSection["ValidIssuer"].ToString(),
                        audience: configurationSection["ValidAudience"].ToString(),
                        expires: DateTime.Now.AddDays(7),
                        claims: tokenClaims,
                        signingCredentials: new SigningCredentials(signInKey, SecurityAlgorithms.HmacSha256));

                    var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

                    var loginCredentials = new LoginCredentialsResponse()
                    {
                        Success = true,
                        AccessToken = accessToken
                    };

                    return loginCredentials;
                }
            }

            return new LoginCredentialsResponse { Success = false, StatusCode = System.Net.HttpStatusCode.NotFound, Error = "User not Found" };
        }
    }
}
