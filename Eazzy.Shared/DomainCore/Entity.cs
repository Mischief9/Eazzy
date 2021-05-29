using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Eazzy.Shared.DomainCore
{
    public class Entity
    {
        [Key]
        public int Id { get; set; }
    }
}
