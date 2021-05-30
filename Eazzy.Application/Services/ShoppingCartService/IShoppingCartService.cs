using Eazzy.Domain.Models.CustomerManagement;
using Eazzy.Domain.Models.MenuManagement;

namespace Eazzy.Application.Services.ShoppingCartService
{
    public interface IShoppingCartService
    {
        void AddToCart(MenuItem item, Customer customer);

        void ClearCart(int customerId);
    }
}
