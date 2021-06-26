using Eazzy.Domain.Models.AccountManagement;
using Eazzy.Domain.Models.CartManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eazzy.Models.Customers
{
    public class CustomerModel
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public int UserId { get; set; }

        public virtual IList<ShoppingCartItem> ShoppingCartItems { get; set; }

        public Role Role { get; set; }
    }
}
