using System;
using System.Collections.Generic;

namespace PhoneStore.Models
{
    public partial class TblCategory
    {
        public TblCategory()
        {
            TblProduct = new HashSet<TblProduct>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool? Status { get; set; }

        public virtual ICollection<TblProduct> TblProduct { get; set; }
    }
}
