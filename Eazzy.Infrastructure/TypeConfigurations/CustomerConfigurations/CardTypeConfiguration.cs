using Eazzy.Domain.Models.CustomerManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eazzy.Infrastructure.TypeConfigurations.CustomerConfigurations
{
    public class CardTypeConfiguration : IEntityTypeConfiguration<Card>
    {
        public void Configure(EntityTypeBuilder<Card> builder)
        {
            builder.ToTable("Cards");

            builder.HasOne(x => x.Customer)
                .WithMany(x => x.Card)
                .HasForeignKey(x => x.CustomerId);
        }
    }
}
