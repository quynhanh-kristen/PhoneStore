using System;
using System.Collections.Generic;

namespace PhoneStore.Models
{
    public partial class TblProduct
    {
        public TblProduct()
        {
            TblOrderDetail = new HashSet<TblOrderDetail>();
        }

        public int Id { get; set; }
        public int? IdCtgPhone { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public int Quantity { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string Configuration { get; set; }
        public int? Rating { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UserCreatedId { get; set; }

        public virtual TblCategory IdCtgPhoneNavigation { get; set; }
        public virtual TblUser UserCreated { get; set; }
        public virtual ICollection<TblOrderDetail> TblOrderDetail { get; set; }
    }
}
