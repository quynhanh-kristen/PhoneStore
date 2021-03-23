using System;
using System.Collections.Generic;

namespace PhoneStore.Models
{
    public partial class TblTempCart
    {
        public int Id { get; set; }
        public int? IdUser { get; set; }
        public int? IdProduct { get; set; }
        public int? TempQuantity { get; set; }

        public virtual TblUser IdUserNavigation { get; set; }
    }
}
