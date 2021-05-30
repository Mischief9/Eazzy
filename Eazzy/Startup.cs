using Autofac.Extensions.DependencyInjection;
using Eazzy.DI;
using Eazzy.Domain.Models.AccountManagement;
using Eazzy.Infrastructure.Models;
using Eazzy.Shared.DomainCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Eazzy
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins, builder =>
                {
                    builder.WithOrigins(Configuration["Cors:AcceptableDomain"])
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .Build();
                });
            });

            services.AddControllers()
                .AddNewtonsoftJson();

            services.AddSwaggerGen(x =>
            {
                x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                x.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new List<string>()
                    }
                });
            });

            services.AddIdentity<User, Role>(opt =>
            {
                opt.SignIn.RequireConfirmedEmail = false;

                opt.Password.RequireDigit = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequiredLength = 6;
            })
            .AddUserStore<UserStore<User, Role, EazzyDbContext, int, UserClaim, UserRole, UserLogin, UserToken,
                RoleClaim>>()
            .AddRoleStore<RoleStore<Role, EazzyDbContext, int, UserRole, RoleClaim>>()
            .AddRoleManager<RoleManager<Role>>()
            .AddSignInManager<SignInManager<User>>()
            .AddUserManager<UserManager<User>>()
            .AddDefaultTokenProviders()
            .AddUserValidator<UserValidator<User>>();

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = Configuration.GetSection("TokenValidationParameters").GetValue<string>("ValidIssuer"),
                    ValidAudience = Configuration.GetSection("TokenValidationParameters").GetValue<string>("ValidAudience"),
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("TokenValidationParameters").GetValue<string>("Key"))),
                };
            });

            new DependenyInjectionResolver(Configuration).Resolve(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //app.UseExceptionHandler(appError =>
                //{
                //    appError.Run(async context =>
                //    {
                //        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                //        context.Response.ContentType = "application/json";

                //        await context.Response.WriteAsync(new FailedResponse
                //        {
                //            Errors = new List<string> { "Internal Server Error." }
                //        }.ToString());
                //    });
                //});
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler();
            }

            app.UseCors(MyAllowSpecificOrigins);

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
