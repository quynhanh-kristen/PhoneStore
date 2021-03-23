using System;
using System.Collections.Generic;

namespace PhoneStore.Models
{
    public partial class TblOrder
    {
        public TblOrder()
        {
            TblOrderDetail = new HashSet<TblOrderDetail>();
        }

        public int Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? IdUser { get; set; }
        public string Payment { get; set; }
        public string Status { get; set; }

        public virtual TblUser IdUserNavigation { get; set; }
        public virtual ICollection<TblOrderDetail> TblOrderDetail { get; set; }
    }
}
