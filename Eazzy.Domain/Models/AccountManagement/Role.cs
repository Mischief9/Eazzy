using Eazzy.Shared.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eazzy.Domain.Models.AccountManagement
{
    public class Role : IdentityRole<int>
    {
        private readonly List<RoleClaim> _roleClaims;

        public Role()
        {
            _roleClaims = _roleClaims ?? new List<RoleClaim>();
        }

        public bool IsDefault { get; set; }

        public virtual IReadOnlyCollection<RoleClaim> RoleClaims => _roleClaims;
    }

    public class RoleClaim : IdentityRoleClaim<int>
    {
        public virtual Role Role { get; set; }

        public Securable Securable { get; set; }
        public Permission Permission { get; set; }
    }
}
