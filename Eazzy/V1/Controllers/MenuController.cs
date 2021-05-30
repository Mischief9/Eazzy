using Eazzy.Application.Models.Menu;
using Eazzy.Application.Services.MenuService;
using Eazzy.Domain.Models.MenuManagement;
using Eazzy.Infrastructure;
using Eazzy.Shared.DomainCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eazzy.V1.Controllers
{
    [Route("v1/menu")]
    public class MenuController : WebApiController
    {
        private readonly IMenuService _menuService;

        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedList<Menu>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetAllRestaurantMenus([FromQuery] GetMenuRequest filter)
        {
            var restaurantMenus = _menuService.GetMenus(filter);

            return Ok(restaurantMenus);
        }
    }
}
