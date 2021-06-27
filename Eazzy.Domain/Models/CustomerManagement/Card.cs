using Eazzy.Shared.DomainCore;
using System;

namespace Eazzy.Domain.Models.CustomerManagement
{
    public class Card : Entity
    {
        public Guid Guid { get; set; }

        public string CardHolder { get; set; }

        public string NumberMask { get; set; }

        public string Expires { get; set; }

        public string CCV { get; set; }

        public int CustomerId { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
