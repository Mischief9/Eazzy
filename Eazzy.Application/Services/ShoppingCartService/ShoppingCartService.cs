using Eazzy.Domain.Models.CartManagement;
using Eazzy.Domain.Models.CustomerManagement;
using Eazzy.Domain.Models.MenuManagement;
using Eazzy.Infrastructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Eazzy.Application.Services.ShoppingCartService
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IRepository<ShoppingCartItem> _shoppingCartItemRepository;

        public ShoppingCartService(IRepository<ShoppingCartItem> shoppingCartItemRepository)
        {
            _shoppingCartItemRepository = shoppingCartItemRepository;
        }

        public List<ShoppingCartItem> GetShoppingCartItems(int customerId)
        {
            var shoppingCartItems = _shoppingCartItemRepository.Table
                .Where(x => x.CustomerId == customerId)
                .Include(x=>x.MenuItem)
                    .ThenInclude(x=>x.Menu)
                .ToList();

            return shoppingCartItems;
        }

        public void AddToCart(MenuItem item, Customer customer)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            var shoppingCartItem = new ShoppingCartItem()
            {
                CustomerId = customer.Id,
                MenuItemId = item.Id,
                Price = item.Price
            };

            _shoppingCartItemRepository.Add(shoppingCartItem);
        }

        public void ClearCart(int customerId)
        {
            var items = _shoppingCartItemRepository.Table.Where(x => x.CustomerId == customerId);

            _shoppingCartItemRepository.Delete(items);
        }
    }
}
