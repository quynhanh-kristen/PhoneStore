using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneStore.Models
{
    public class CartItem
    {
        public int quantity { set; get; }
        public TblProduct product { set; get; }
    }
}
