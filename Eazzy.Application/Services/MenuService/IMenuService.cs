using Eazzy.Application.Models.Menu;
using Eazzy.Domain.Models.MenuManagement;
using Eazzy.Models.Menu;
using Eazzy.Shared.DomainCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Eazzy.Application.Services.MenuService
{
    public interface IMenuService
    {
        Menu FindById(int id);

        void InsertMenu(Menu menu);

        void InsertMenu(IEnumerable<Menu> menus);

        void UpdateMenu(Menu menu);

        void DeleteMenu(Menu menu);

        IPagedList<Menu> GetMenus(GetMenuRequest request);

        MenuItem GetMenuItemById(int id);

        IPagedList<MenuItem> GetMenuItems(int menuId, GetMenuItemsRequest request);

        void InsertMenuItem(MenuItem menuItem);

        void InsertMenuItem(IEnumerable<MenuItem> menuItems);

        void UpdateMenuItem(MenuItem menuItem);

        void DeleteMenuItem(MenuItem menuItem);

        IPagedList<MenuItemType> GetMenuItemTypes(GetMenuItemTypesRequest request);

        MenuItemType GetMenuItemTypeById(int id);

        void InsertMenuItemType(MenuItemType menuItemType);

        void InsertMenuItemType(IEnumerable<MenuItemType> menuItemTypes);

        void UpdateMenuItemType(MenuItemType menuItemType);

        void DeleteMenuItemType(MenuItemType menuItemType);
    }
}
