using Eazzy.Domain.Models.AccountManagement;
using Eazzy.Domain.Models.CustomerManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eazzy.Infrastructure.TypeConfigurations.CustomerConfigurations
{
    public class CustomerTypeConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customers");

            builder.HasKey(x => x.Id);


            builder.HasOne(x => x.User)
                .WithOne(x => x.Customer)
                .HasForeignKey<User>(x => x.CustomerId);
        }
    }
}
