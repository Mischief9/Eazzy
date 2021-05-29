using Eazzy.Domain.Models.CustomerManagement;
using Eazzy.Domain.Models.TenantManagement;
using Eazzy.Shared.DomainCore.Interface;
using Microsoft.AspNetCore.Identity;

namespace Eazzy.Domain.Models.AccountManagement
{
    public class User : IdentityUser<int>
    {
        public bool IsDeleted { get; set; }

        public int CustomerId { get; set; }

        public virtual Customer Customer { get; set; }

        public int? TenantId { get; set; }

        public virtual Tenant Tenant { get; set; }
    }

    public class UserClaim : IdentityUserClaim<int>
    {
        public int RoleId { get; set; }

        public virtual Role Role { get; set; }
    }

    public class UserLogin : IdentityUserLogin<int>
    {
        public virtual int Id { get; set; }
    }

    public class UserRole : IdentityUserRole<int>
    {
        public virtual int Id { get; set; }

        public virtual Role Role { get; set; }

        public virtual User User { get; set; }
    }

    public class UserToken : IdentityUserToken<int>
    {
        public virtual int Id { get; set; }
    }
}
