using Eazzy.Infrastructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Eazzy.Domain.Models.AccountManagement;
using System.Threading.Tasks;
using Eazzy.Infrastructure.Models;
using System.Linq;

namespace Eazzy.Application.Services.RoleService
{
    public class RoleService : IRoleService
    {
        private EazzyDbContext _db;

        public RoleService(EazzyDbContext db)
        {
            _db = db;
        }

        public Role FindById(int id)
        {
            var role = _db.Set<Role>().Find(id);

            return role;
        }

        public void InsertRole(Role role)
        {
            if (role == null)
                throw new ArgumentNullException();

            _db.Set<Role>().Add(role);
        }

        public void InsertRole(IEnumerable<Role> roles)
        {
            if (roles == null)
                throw new ArgumentNullException();

            _db.Set<Role>().AddRange(roles);
        }

        public void UpdateRole(Role role)
        {
            if (role == null)
                throw new ArgumentNullException();

            _db.Set<Role>().Update(role);
        }

        public void DeleteRole(Role role)
        {
            if (role == null)
                throw new ArgumentNullException();

            _db.Set<Role>().Remove(role);
        }

        public Role FindByUserId(int userId)
        {
            var userRole = _db.Set<UserRole>().FirstOrDefault(x=>x.UserId == userId);

            if(userRole == null)
                throw new ArgumentNullException(nameof(userRole));

            return userRole.Role;
        }
    }
}
