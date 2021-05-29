using Eazzy.Domain.Models.TenantManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eazzy.Infrastructure.TypeConfigurations.TenantConfigurations
{
    public class TenantTypeConfiguration : IEntityTypeConfiguration<Tenant>
    {
        public void Configure(EntityTypeBuilder<Tenant> builder)
        {
            builder.ToTable("Tenants");
            builder.HasKey(x => x.Id);

            builder.HasMany(x => x.Tables)
                .WithOne(x => x.Tenant)
                .HasForeignKey(x => x.TenantId);
        }
    }
}
