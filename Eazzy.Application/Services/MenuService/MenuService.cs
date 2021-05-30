using Eazzy.Application.Models.Menu;
using Eazzy.Domain.Models.MenuManagement;
using Eazzy.Infrastructure.Repository.Interfaces;
using Eazzy.Shared.DomainCore;
using Microsoft.EntityFrameworkCore;
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
        private readonly IRepository<MenuItem> _menuItemRepository;

        public MenuService(IRepository<Menu> menuRepository,
            IRepository<MenuItem> menuItemRepository)
        {
            _menuRepository = menuRepository;
            _menuItemRepository = menuItemRepository;
        }

        public void DeleteMenu(Menu menu)
        {
            if (menu == null)
                throw new ArgumentNullException(nameof(menu));

            _menuRepository.Delete(menu);
        }

        public void DeleteMenuItem(MenuItem menuItem)
        {
            if (menuItem == null)
                throw new ArgumentNullException(nameof(menuItem));

            _menuItemRepository.Delete(menuItem);
        }

        public Menu FindById(int id)
        {
            var menu = _menuRepository.Find(id);

            return menu;
        }

        public MenuItem GetMenuItemById(int id)
        {
            var menuItem = _menuItemRepository.Find(id);

            return menuItem;
        }

        public IPagedList<Menu> GetMenus(GetMenuRequest request)
        {
            var menus = _menuRepository.Table.Include(x => x.MenuItems);

            if (request.TenantId.HasValue)
            {
                menus.Where(x => x.TenantId == request.TenantId.Value);
            }

            return new PagedList<Menu>(menus, request.PageIndex, request.PageSize);
        }

        public void InsertMenu(Menu menu)
        {
            if (menu == null)
                throw new ArgumentNullException(nameof(menu));

            _menuRepository.Add(menu);
        }

        public void InsertMenu(IEnumerable<Menu> menus)
        {
            if (menus == null)
                throw new ArgumentNullException(nameof(menus));

            _menuRepository.Add(menus);
        }

        public void InsertMenuItem(MenuItem menuItem)
        {
            if (menuItem == null)
                throw new ArgumentNullException(nameof(menuItem));

            _menuItemRepository.Add(menuItem);
        }

        public void InsertMenuItem(IEnumerable<MenuItem> menuItems)
        {
            if (menuItems == null)
                throw new ArgumentNullException(nameof(menuItems));

            _menuItemRepository.Add(menuItems);
        }

        public void UpdateMenu(Menu menu)
        {
            if (menu == null)
                throw new ArgumentNullException(nameof(menu));

            _menuRepository.Update(menu);
        }

        public void UpdateMenuItem(MenuItem menuItem)
        {
            if (menuItem == null)
                throw new ArgumentNullException(nameof(menuItem));

            _menuItemRepository.Update(menuItem);
        }
    }
}
