using System;
using System.Collections.Generic;

namespace PhoneStore.Models
{
    public partial class TblUser
    {
        public TblUser()
        {
            TblOrder = new HashSet<TblOrder>();
            TblProduct = new HashSet<TblProduct>();
            TblTempCart = new HashSet<TblTempCart>();
        }

        public int Id { get; set; }
        public int? IdRole { get; set; }
        public string Fullname { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public bool? Status { get; set; }

        public virtual TblRole IdRoleNavigation { get; set; }
        public virtual ICollection<TblOrder> TblOrder { get; set; }
        public virtual ICollection<TblProduct> TblProduct { get; set; }
        public virtual ICollection<TblTempCart> TblTempCart { get; set; }
    }
}
