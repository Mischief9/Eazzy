using Autofac.Extensions.DependencyInjection;
using Eazzy.DI;
using Eazzy.Domain.Models.AccountManagement;
using Eazzy.Domain.Models.CustomerManagement;
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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
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
                    var acceptableDomains = new string[] { Configuration["Cors:AcceptableDomain"] };
                    builder.WithOrigins(acceptableDomains)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .Build();
                });
            });

            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });

            services.AddSwaggerGenNewtonsoftSupport();
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

            app.UseRouting();

            app.UseCors(MyAllowSpecificOrigins);

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            Migrate(app);
            Seed(app);
        }

        private void Migrate(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var context = serviceScope.ServiceProvider.GetService<EazzyDbContext>();
            context.Database.Migrate();
        }

        private void Seed(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var context = serviceScope.ServiceProvider.GetService<EazzyDbContext>();
            var userManager = serviceScope.ServiceProvider.GetService<UserManager<User>>();

            // add administrator role if doesnt exist.
            var administratorRoles = context.Set<Role>().Where(x => x.Name == "Administrator").ToList();

            if (!administratorRoles.Any())
            {
                var newRole = new Role()
                {
                    Name = "Administrator",
                    NormalizedName = "Administrator"
                };

                context.Set<Role>().Add(newRole);

                context.SaveChanges();
            }

            // add system administrator role if doesnt exist.
            var sysAdministratorRoles = context.Set<Role>().Where(x => x.Name == "System Administrator").ToList();

            if (!sysAdministratorRoles.Any())
            {
                var newRole = new Role()
                {
                    Name = "System Administrator",
                    NormalizedName = "System Administrator"
                };

                context.Set<Role>().Add(newRole);

                context.SaveChanges();
            }

            // add system administrator user.
            var newUser =  userManager.CreateAsync(new User()
            {
                Email = "admin@eazzy.ge",
                UserName = "SysAdmin",
                Customer = new Customer()
                {
                    FirstName = "admin",
                    LastName = "admin",
                    PhoneNumber = "555123456789"
                }
            },
            "123456").Result;

            // add system administrator and user relationship.
            var sysAdministrator = context.Set<Role>().First(x => x.Name == "System Administrator");
            var sysUser = context.Set<User>().First(x => x.UserName == "SysAdmin");
            
            var sysAdminUserRole = new UserRole()
            {
                RoleId = sysAdministrator.Id,
                UserId = sysUser.Id
            };

            context.Set<UserRole>().Add(sysAdminUserRole);

            context.SaveChanges();
        }
    }
}
