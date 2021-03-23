using System;
using System.Collections.Generic;

namespace PhoneStore.Models
{
    public partial class TblOrderDetail
    {
        public int Id { get; set; }
        public int? IdProduct { get; set; }
        public int? IdOrder { get; set; }
        public int? BoughtQuantity { get; set; }
        public double? Tax { get; set; }

        public virtual TblOrder IdOrderNavigation { get; set; }
        public virtual TblProduct IdProductNavigation { get; set; }
    }
}
