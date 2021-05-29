using Eazzy.Domain.Models.AccountManagement;
using Eazzy.Infrastructure.Models;
using Eazzy.Infrastructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Autofac;
using Autofac.Builder;
using Autofac.Core;
using Eazzy.Infrastructure.Repository.Abstract;
using Autofac.Extensions.DependencyInjection;
using System;
using Eazzy.Application.Services.RoleService;
using Eazzy.Application.Services.AccountService;
using Eazzy.Application.Services.CustomerService;
using Eazzy.Application.Services.OrderService;
using Eazzy.Application.Services.MenuService;

namespace Eazzy.DI
{
    public class DependenyInjectionResolver
    {
        private readonly IConfiguration _configuration;

        public DependenyInjectionResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Resolve(IServiceCollection services)
        {
            services.AddDbContext<EazzyDbContext>(
                options => options.UseSqlServer(_configuration.GetConnectionString("EazzyDbContext")));

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IMenuService, MenuService>();
        }
    }
}
