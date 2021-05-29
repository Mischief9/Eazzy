using Eazzy.Domain.Models.MenuManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eazzy.Infrastructure.TypeConfigurations.MenuConfiguration
{
    public class MenuItemTypeConfiguration : IEntityTypeConfiguration<MenuItem>
    {
        public void Configure(EntityTypeBuilder<MenuItem> builder)
        {
            builder.ToTable("MenuItems");
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Menu)
                .WithMany(x => x.MenuItems)
                .HasForeignKey(x => x.MenuId);
        }
    }
}
