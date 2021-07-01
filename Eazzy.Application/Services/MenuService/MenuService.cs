using Eazzy.Application.Models.Menu;
using Eazzy.Domain.Models.MenuManagement;
using Eazzy.Infrastructure.Repository.Interfaces;
using Eazzy.Models.Menu;
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
        private readonly IRepository<MenuItemType> _menuItemTypeRepository;

        public MenuService(IRepository<Menu> menuRepository,
            IRepository<MenuItem> menuItemRepository,
            IRepository<MenuItemType> menuItemTypeRepository)
        {
            _menuRepository = menuRepository;
            _menuItemRepository = menuItemRepository;
            _menuItemTypeRepository = menuItemTypeRepository;
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

        public IPagedList<MenuItem> GetMenuItems(int menuId, GetMenuItemsRequest request)
        {
            var menuItems = _menuItemRepository.Table.Where(x=>x.MenuId == menuId);

            if(request.MenuItemTypeIds != null && request.MenuItemTypeIds.Any())
            {
                menuItems = menuItems.Where(x => request.MenuItemTypeIds.Contains(x.MenuItemTypeId));
            }

            return new PagedList<MenuItem>(menuItems, request.PageIndex, request.PageSize);
        }

        public MenuItem GetMenuItemById(int id)
        {
            var menuItem = _menuItemRepository.Find(id);

            return menuItem;
        }

        public IPagedList<Menu> GetMenus(GetMenuRequest request)
        {
            var menus = _menuRepository.Table.AsQueryable();

            if (request.TenantId.HasValue)
            {
                menus = menus.Where(x => x.TenantId == request.TenantId.Value);
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

        public IPagedList<MenuItemType> GetMenuItemTypes(GetMenuItemTypesRequest request)
        {
            var menuItemTypes = _menuItemTypeRepository.Table;

            if (!string.IsNullOrEmpty(request.Name))
            {
                menuItemTypes = menuItemTypes.Where(x => x.Name.Contains(request.Name));
            }

            return new PagedList<MenuItemType>(menuItemTypes, request.PageIndex, request.PageSize);
        }

        public MenuItemType GetMenuItemTypeById(int id)
        {
            var menuItemType = _menuItemTypeRepository.Find(id);

            return menuItemType;
        }

        public void InsertMenuItemType(MenuItemType menuItemType)
        {
            if (menuItemType == null)
                throw new ArgumentNullException(nameof(menuItemType));

            _menuItemTypeRepository.Add(menuItemType);

        }

        public void InsertMenuItemType(IEnumerable<MenuItemType> menuItemTypes)
        {
            if (menuItemTypes == null)
                throw new ArgumentNullException(nameof(menuItemTypes));

            _menuItemTypeRepository.Add(menuItemTypes);
        }

        public void UpdateMenuItemType(MenuItemType menuItemType)
        {
            if (menuItemType == null)
                throw new ArgumentNullException(nameof(menuItemType));

            _menuItemTypeRepository.Update(menuItemType);
        }

        public void DeleteMenuItemType(MenuItemType menuItemType)
        {
            if (menuItemType == null)
                throw new ArgumentNullException(nameof(menuItemType));

            _menuItemTypeRepository.Delete(menuItemType);
        }
    }
}
