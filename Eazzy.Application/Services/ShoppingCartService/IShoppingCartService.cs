﻿using Eazzy.Domain.Models.CartManagement;
using Eazzy.Domain.Models.CustomerManagement;
using Eazzy.Domain.Models.MenuManagement;
using System.Collections.Generic;

namespace Eazzy.Application.Services.ShoppingCartService
{
    public interface IShoppingCartService
    {
        List<ShoppingCartItem> GetShoppingCartItems(int customerId);

        void AddToCart(MenuItem item, Customer customer);

        void ClearCart(int customerId);
    }
}
