using Eazzy.Application.Services.CustomerService;
using Eazzy.Application.Services.MenuService;
using Eazzy.Application.Services.ShoppingCartService;
using Eazzy.Infrastructure;
using Eazzy.Shared.DomainCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Eazzy.V1.Controllers
{
    [Route("v1/cart")]
    public class ShoppingCartController : WebApiController
    {
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IMenuService _menuService;
        private readonly ICustomerService _customerService;

        public ShoppingCartController(IShoppingCartService shoppingCartService,
            ICustomerService customerService,
            IMenuService menuService)
        {
            _shoppingCartService = shoppingCartService;
            _customerService = customerService;
            _menuService = menuService;
        }

        [HttpPost("add")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(FailedResponse),StatusCodes.Status400BadRequest)]
        public IActionResult AddItemToCart([FromQuery]int menuItemId)
        {
            var username = User.Identity.Name;
            var customer = _customerService.FindByUserName(username);

            var menuItem = _menuService.GetMenuItemById(menuItemId);

            if (menuItem == null)
            {
                return Fail(HttpStatusCode.NotFound, "Menu wasn't found.");
            }

            _shoppingCartService.AddToCart(menuItem, customer);

            return NoContent();
        }

        [HttpPost("clear")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status400BadRequest)]
        public IActionResult ClearCart()
        {
            var username = User.Identity.Name;
            var customer = _customerService.FindByUserName(username);

            _shoppingCartService.ClearCart(customer.Id);

            return NoContent();
        }
    }
}
