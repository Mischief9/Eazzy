using Eazzy.Application.Models.Menu;
using Eazzy.Application.Services.CustomerService;
using Eazzy.Application.Services.MenuService;
using Eazzy.Domain.Models.MenuManagement;
using Eazzy.Infrastructure;
using Eazzy.Models.Menu;
using Eazzy.Shared.DomainCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Eazzy.V1.Controllers
{
    [Route("v1/menu")]
    public class MenuController : WebApiController
    {
        private readonly IMenuService _menuService;
        private readonly ICustomerService _customerService;

        public MenuController(IMenuService menuService,
            ICustomerService customerService)
        {
            _menuService = menuService;
            _customerService = customerService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedList<Menu>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status400BadRequest)]
        public IActionResult GetAllRestaurantMenus([FromQuery] GetMenuRequest filter)
        {
            var restaurantMenus = _menuService.GetMenus(filter);

            return Ok(new
            {
                data = restaurantMenus.Data,
                totalCount = restaurantMenus.TotalCount
            });
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status404NotFound)]
        // [Authorize(Roles = "Administrator")]
        public IActionResult AddMenu([FromBody] AddOrUpdateMenuModel model)
        {
            var customer = GetCurrentCustomer();
            var tenantId = customer.User.TenantId;

            if (!tenantId.HasValue)
                return Fail(HttpStatusCode.NotFound, "Tenant wasn't found");

            var menu = new Menu()
            {
                Name = model.Name,
                TenantId = tenantId.Value,
                CreateDateOnUtc = DateTime.UtcNow,
                UpdatedDateOnUtc = DateTime.UtcNow
            };

            _menuService.InsertMenu(menu);

            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Menu), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status404NotFound)]
        public IActionResult GetMenu([FromRoute] int id)
        {
            var menu = _menuService.FindById(id);

            if (menu == null)
            {
                return Fail(HttpStatusCode.NotFound, "Menu wasn't found.");
            }

            return Ok(menu);
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(Menu), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status404NotFound)]
        // [Authorize(Roles = "Administrator")]
        public IActionResult ChangeMenu([FromRoute] int id, [FromBody] AddOrUpdateMenuModel model)
        {
            var menu = _menuService.FindById(id);

            if (menu == null)
            {
                return Fail(HttpStatusCode.NotFound, "Menu wasn't found.");
            }

            menu.Name = model.Name;

            _menuService.UpdateMenu(menu);

            return Ok(menu);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status404NotFound)]
        public IActionResult DeleteMenu([FromRoute] int id)
        {
            var menu = _menuService.FindById(id);

            if (menu == null)
            {
                return Fail(HttpStatusCode.NotFound, "Menu wasn't found.");
            }

            _menuService.DeleteMenu(menu);

            return NoContent();
        }

        [HttpGet("menuitem/{menuItemId}")]
        [ProducesResponseType(typeof(MenuItem), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status404NotFound)]
        public IActionResult GetMenuItem([FromRoute] int menuItemId)
        {
            var menuItem = _menuService.GetMenuItemById(menuItemId);

            if (menuItem == null)
            {
                return Fail(HttpStatusCode.NotFound, "Menu item wasn't found.");
            }

            return Ok(menuItem);
        }

        [HttpPost("menuitem")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status400BadRequest)]
        // [Authorize(Roles = "Administrator")]
        public IActionResult AddMenuItem([FromBody] AddOrUpdateMenuItemModel model)
        {
            var menuItem = new MenuItem()
            {
                Name = model.Name,
                Description = model.Description,
                MenuItemTypeId = model.MenuItemTypeId,
                Price = model.Price,
                MenuId = model.MenuId
            };

            _menuService.InsertMenuItem(menuItem);

            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPatch("menuitem/{menuItemId}")]
        [ProducesResponseType(typeof(MenuItem), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status404NotFound)]
        // [Authorize(Roles = "Administrator")]
        public IActionResult ChangeMenuItem([FromRoute] int menuItemId, [FromBody] AddOrUpdateMenuItemModel model)
        {
            var menuItem = _menuService.GetMenuItemById(menuItemId);

            if (menuItem == null)
            {
                return Fail(HttpStatusCode.NotFound, "Menu item wasn't found.");
            }

            menuItem.Name = model.Name;
            menuItem.Description = model.Description;
            menuItem.MenuItemTypeId = model.MenuItemTypeId;
            menuItem.Price = model.Price;

            _menuService.UpdateMenuItem(menuItem);

            return Ok(menuItem);
        }

        [HttpDelete("menuitem/{menuItemId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status404NotFound)]
        // [Authorize(Roles = "Administrator")]
        public IActionResult DeleteMenuItem([FromRoute] int menuItemId)
        {
            var menuItem = _menuService.GetMenuItemById(menuItemId);

            if (menuItem == null)
            {
                return Fail(HttpStatusCode.NotFound, "Menu item wasn't found.");
            }

            _menuService.DeleteMenuItem(menuItem);

            return NoContent();
        }

        [HttpGet("menuitemtypes")]
        [ProducesResponseType(typeof(PagedList<MenuItemType>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status404NotFound)]
        public IActionResult GetMenuItemTypes([FromQuery] GetMenuItemTypesModel model)
        {
            var request = new GetMenuItemTypesRequest()
            {
                Name = model.Name,
                Sort = model.Sort,
                SortBy = model.SortBy,
                PageSize = model.PageSize,
                PageIndex = model.PageIndex
            };

            var menuItemTypes = _menuService.GetMenuItemTypes(request);

            return Ok( new
            {
                data = menuItemTypes.Data,
                totalCount = menuItemTypes.TotalCount
            });
        }

        [HttpGet("menuitemtype/{menuItemTypeId}")]
        [ProducesResponseType(typeof(MenuItemType), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status404NotFound)]
        public IActionResult GetMenuItemType([FromRoute] int menuItemTypeId)
        {
            var menuItemType = _menuService.GetMenuItemTypeById(menuItemTypeId);

            if (menuItemType == null)
            {
                return Fail(HttpStatusCode.NotFound, "Menu Item Type item wasn't found.");
            }

            return Ok(menuItemType);
        }

        [HttpPost("menuitemtype")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status400BadRequest)]
        // [Authorize(Roles = "Administrator")]
        public IActionResult AddMenuItemType([FromBody] AddOrUpdateMenuItemType model)
        {
            var menuItemType = new MenuItemType()
            {
                Name = model.Name
            };

            _menuService.InsertMenuItemType(menuItemType);

            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPatch("menuitemtype/{menuItemTypeId}")]
        [ProducesResponseType(typeof(MenuItemType), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status404NotFound)]
        // [Authorize(Roles = "Administrator")]
        public IActionResult ChangeMenuItemType([FromRoute] int menuItemTypeId, [FromBody] AddOrUpdateMenuItemModel model)
        {
            var menuItemType = _menuService.GetMenuItemTypeById(menuItemTypeId);

            if (menuItemType == null)
            {
                return Fail(HttpStatusCode.NotFound, "Menu item wasn't found.");
            }

            menuItemType.Name = model.Name;

            _menuService.UpdateMenuItemType(menuItemType);

            return Ok(menuItemType);
        }

        [HttpDelete("menuitemtype/{menuItemId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status404NotFound)]
        // [Authorize(Roles = "Administrator")]
        public IActionResult DeleteMenuItemType([FromRoute] int menuItemTypeId)
        {
            var menuItemType = _menuService.GetMenuItemTypeById(menuItemTypeId);

            if (menuItemType == null)
            {
                return Fail(HttpStatusCode.NotFound, "Menu item wasn't found.");
            }

            _menuService.DeleteMenuItemType(menuItemType);

            return NoContent();
        }
    }
}
