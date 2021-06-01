using Eazzy.Application.Services.CustomerService;
using Eazzy.Application.Services.MenuService;
using Eazzy.Application.Services.RestaurantService;
using Eazzy.Application.Services.ShoppingCartService;
using Eazzy.Domain.Models.CartManagement;
using Eazzy.Infrastructure;
using Eazzy.Models.Cart;
using Eazzy.Shared.DomainCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Eazzy.V1.Controllers
{
    [Route("v1/cart")]
    public class ShoppingCartController : WebApiController
    {
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IMenuService _menuService;
        private readonly ICustomerService _customerService;
        private readonly IRestaurantService _restaurantService;

        public ShoppingCartController(IShoppingCartService shoppingCartService,
            ICustomerService customerService,
            IMenuService menuService,
            IRestaurantService restaurantService)
        {
            _shoppingCartService = shoppingCartService;
            _customerService = customerService;
            _menuService = menuService;
            _restaurantService = restaurantService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(CartDetails), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
        public IActionResult Cart()
        {
            var username = User.Identity.Name;
            var customer = _customerService.FindByUserName(username);

            var shoppingItems = _shoppingCartService.GetShoppingCartItems(customer.Id);

            if (!shoppingItems.Any())
            {
                return NotFound();
            }

            var tenantId = shoppingItems.Select(x => x.MenuItem.Menu.TenantId).First();

            var orderTotal = shoppingItems.Sum(x => x.Price);
            orderTotal = _restaurantService.GetRestaurantOrderTotalAndTax(tenantId, orderTotal, out decimal tax);

            var cartDetails = new CartDetails()
            {
                CartItems = shoppingItems.Select(x => new CartItem()
                {
                    Name = x.MenuItem.Name,
                    MenuItemId = x.MenuItemId,
                    Price = x.Price,
                    CustomerId = x.CustomerId
                }).ToList(),
                OrderTotal = orderTotal,
                Tax = tax,
                TenantId = tenantId
            };

            return Ok(cartDetails);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status404NotFound)]
        // [Authorize]
        public IActionResult AddItemToCart([FromBody] AddOrDeleteItemToCartModel model)
        {
            var username = User.Identity.Name;
            var customer = _customerService.FindByUserName(username);

            var menuItem = _menuService.GetMenuItemById(model.MenuItemId);

            if (menuItem == null)
            {
                return Fail(HttpStatusCode.NotFound, "Menu wasn't found.");
            }

            _shoppingCartService.AddToCart(menuItem, customer);

            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status404NotFound)]
        // [Authorize]
        public IActionResult RemoveItemFromCart([FromRoute] int id)
        {
            var customer = GetCurrentCustomer();

            var shoppingCartItem = _shoppingCartService.GetByMenuItemIdAndCustomer(id, customer.Id);

            if (shoppingCartItem == null)
            {
                return Fail(HttpStatusCode.NotFound, "Item in the cart wasn't found.");
            }

            _shoppingCartService.RemoveFromCart(shoppingCartItem);

            return NoContent();
        }

        [HttpDelete("clear")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(FailedResponse), StatusCodes.Status400BadRequest)]
        // [Authorize]
        public IActionResult ClearCart()
        {
            var username = User.Identity.Name;
            var customer = _customerService.FindByUserName(username);

            _shoppingCartService.ClearCart(customer.Id);

            return NoContent();
        }
    }
}
