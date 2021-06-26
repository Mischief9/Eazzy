using Eazzy.Domain.Models.AccountManagement;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Eazzy.Application.Services.RoleService
{
    public interface IRoleService
    {
        Role FindById(int id);

        void InsertRole(Role role);

        void InsertRole(IEnumerable<Role> roles);

        void UpdateRole(Role role);

        void DeleteRole(Role role);

        Role FindByUserId(int userId);
    }
}
