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

            return Ok(restaurantMenus);
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

            if(menu == null)
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
        public IActionResult ChangeMenu([FromRoute] int id,[FromBody] AddOrUpdateMenuModel model)
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

            if(menu == null)
            {
                return Fail(HttpStatusCode.NotFound, "Menu wasn't found.");
            }

            _menuService.DeleteMenu(menu);

            return NoContent();
        }

        [HttpGet("menuitem/{menuItemId}")]
        [ProducesResponseType(typeof(MenuItem),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status404NotFound)]
        public IActionResult GetMenuItem([FromRoute]int menuItemId)
        {
            var menuItem = _menuService.GetMenuItemById(menuItemId);

            if(menuItem == null)
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
                MenuItemType = model.MenuItemType,
                Price = model.Price,
                MenuId = model.MenuId
            };

            _menuService.InsertMenuItem(menuItem);

            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPatch("menuitem/{menuItemId}")]
        [ProducesResponseType(typeof(MenuItem),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status404NotFound)]
        // [Authorize(Roles = "Administrator")]
        public IActionResult ChangeMenuItem([FromRoute] int menuItemId, [FromBody] AddOrUpdateMenuItemModel model)
        {
            var menuItem = _menuService.GetMenuItemById(menuItemId);

            if(menuItem == null)
            {
                return Fail(HttpStatusCode.NotFound, "Menu item wasn't found.");
            }

            menuItem.Name = model.Name;
            menuItem.Description = model.Description;
            menuItem.MenuItemType = model.MenuItemType;
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
    }
}
