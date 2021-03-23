using System;
using System.Collections.Generic;

namespace PhoneStore.Models
{
    public partial class TblRole
    {
        public TblRole()
        {
            TblUser = new HashSet<TblUser>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool? Status { get; set; }

        public virtual ICollection<TblUser> TblUser { get; set; }
    }
}
