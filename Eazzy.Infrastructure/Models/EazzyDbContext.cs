using Eazzy.Domain.Models.TenantManagement;
using Eazzy.Infrastructure.TypeConfigurations.AccountConfigurations;
using Eazzy.Infrastructure.TypeConfigurations.CustomerConfigurations;
using Eazzy.Infrastructure.TypeConfigurations.MenuConfiguration;
using Eazzy.Infrastructure.TypeConfigurations.OrderConfiguration;
using Eazzy.Infrastructure.TypeConfigurations.PaymentTransactionConfiguration;
using Eazzy.Infrastructure.TypeConfigurations.TableConfigurations;
using Eazzy.Infrastructure.TypeConfigurations.TenantConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Eazzy.Infrastructure.Models
{
    public class EazzyDbContext : DbContext
    {
        public EazzyDbContext(DbContextOptions options)
            : base(options)
        {
            //Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tenant>().HasQueryFilter(x => !x.IsDeleted);

            modelBuilder.ApplyConfiguration(new UserTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserClaimTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserTokenTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserLoginTypeConfiguration());
            modelBuilder.ApplyConfiguration(new RoleTypeConfiguration());
            modelBuilder.ApplyConfiguration(new RoleClaimTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CustomerTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CardTypeConfiguration());
            modelBuilder.ApplyConfiguration(new MenuTypeConfiguration());
            modelBuilder.ApplyConfiguration(new MenuItemTypeConfiguration());
            modelBuilder.ApplyConfiguration(new OrderTypeConfiguration());
            modelBuilder.ApplyConfiguration(new OrderItemTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PaymentTransactionTypeConfiguration());
            modelBuilder.ApplyConfiguration(new TableTypeConfiguration());
            modelBuilder.ApplyConfiguration(new TenantTypeConfiguration());
        }
    }
}
