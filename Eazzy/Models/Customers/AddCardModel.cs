using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eazzy.Models.Customers
{
    public class AddCardModel
    {
        public string NumberMask { get; set; }

        public string CardHolder { get; set; }

        public string CCV { get; set; }

        public string Expires { get; set; }
    }
}
