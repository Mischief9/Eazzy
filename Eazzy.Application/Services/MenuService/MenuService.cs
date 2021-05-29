using Eazzy.Application.Models.Menu;
using Eazzy.Domain.Models.MenuManagement;
using Eazzy.Infrastructure.Repository.Interfaces;
using Eazzy.Shared.DomainCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eazzy.Application.Services.MenuService
{
    public class MenuService : IMenuService
    {
        private readonly IRepository<Menu> _menuRepository;

        public MenuService(IRepository<Menu> menuRepository)
        {
            _menuRepository = menuRepository;
        }

        public void DeleteMenu(Menu menu)
        {
            throw new NotImplementedException();
        }

        public Menu FindById(int id)
        {
            var menu = _menuRepository.Find(id);

            return menu;
        }

        public IPagedList<Menu> GetMenus(GetMenuRequest request)
        {
            var menus = _menuRepository.Table;

            if (request.TenantId.HasValue)
            {
                menus.Where(x => x.TenantId == request.TenantId.Value);
            }

            return new PagedList<Menu>(menus, request.PageIndex, request.PageSize);
        }

        public void InsertMenu(Menu menu)
        {
            throw new NotImplementedException();
        }

        public void InsertMenu(IEnumerable<Menu> menus)
        {
            throw new NotImplementedException();
        }

        public void UpdateMenu(Menu menu)
        {
            throw new NotImplementedException();
        }
    }
}
