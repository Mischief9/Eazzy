using Eazzy.Domain.Models.RestaurantManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eazzy.Infrastructure.TypeConfigurations.TableConfigurations
{
    public class TableTypeConfiguration : IEntityTypeConfiguration<Table>
    {
        public void Configure(EntityTypeBuilder<Table> builder)
        {
            builder.ToTable("Tables");
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Tenant)
                .WithMany(x => x.Tables)
                .HasForeignKey(x => x.TenantId);
        }
    }
}
