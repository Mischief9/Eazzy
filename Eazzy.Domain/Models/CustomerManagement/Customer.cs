using Eazzy.Domain.Models.AccountManagement;
using Eazzy.Domain.Models.CartManagement;
using Eazzy.Shared.DomainCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Eazzy.Domain.Models.CustomerManagement
{
    public class Customer : Entity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public int UserId { get; set; }

        public virtual User User { get; set; }

        public virtual ICollection<ShoppingCartItem> ShoppingCartItems { get; set; }

        public string GetFullName() => $"{FirstName} {LastName}";
    }
}
