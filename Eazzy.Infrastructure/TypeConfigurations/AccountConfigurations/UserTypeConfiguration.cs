using Eazzy.Domain.Models.AccountManagement;
using Eazzy.Domain.Models.CustomerManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eazzy.Infrastructure.TypeConfigurations.AccountConfigurations
{
    public class UserTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Customer)
                .WithOne(x => x.User)
                .HasForeignKey<Customer>(x=>x.UserId);
        }
    }

    public class UserLoginTypeConfiguration : IEntityTypeConfiguration<UserLogin>
    {
        public void Configure(EntityTypeBuilder<UserLogin> builder)
        {
            builder.ToTable("UserLogins");

            builder.HasKey(x => x.Id);
        }
    }

    public class UserClaimTypeConfiguration : IEntityTypeConfiguration<UserClaim>
    {
        public void Configure(EntityTypeBuilder<UserClaim> builder)
        {
            builder.ToTable("UserClaims");

            builder.HasKey(x => x.Id);
        }
    }

    public class UserRoleTypeConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("UserRoles");

            builder.HasKey(x => x.Id);
        }
    }

    public class UserTokenTypeConfiguration : IEntityTypeConfiguration<UserToken>
    {
        public void Configure(EntityTypeBuilder<UserToken> builder)
        {
            builder.ToTable("UserTokens");

            builder.HasKey(x => x.Id);
        }
    }
}
