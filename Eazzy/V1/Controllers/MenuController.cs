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
    // [Authorize(Roles = "Administrator")]
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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetAllRestaurantMenus([FromQuery] GetMenuRequest filter)
        {
            var restaurantMenus = _menuService.GetMenus(filter);

            return Ok(restaurantMenus);
        }

        [HttpPost("add")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult AddMenu([FromBody] AddMenuModel model)
        {
            var username = User.Identity.Name;
            var customer = _customerService.FindByUserName(username);
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

        [HttpPost("add/menuitem")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult AddMenuItem([FromBody] AddMenuItemModel model)
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
    }
}
