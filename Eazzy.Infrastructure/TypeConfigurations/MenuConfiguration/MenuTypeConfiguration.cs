using Eazzy.Domain.Models.MenuManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eazzy.Infrastructure.TypeConfigurations.MenuConfiguration
{
    public class MenuTypeConfiguration : IEntityTypeConfiguration<Menu>
    {
        public void Configure(EntityTypeBuilder<Menu> builder)
        {
            builder.ToTable("Menus");
            builder.HasKey(x => x.Id);

            builder.HasMany(x => x.MenuItems)
                .WithOne(x => x.Menu)
                .HasForeignKey(x => x.MenuId);

            builder.HasOne(x => x.Tenant)
                .WithMany(x=>x.Menus)
                .HasForeignKey(x=>x.TenantId);
        }
    }
}
