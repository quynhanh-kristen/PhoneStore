using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        [Required]
        public string Fullname { get; set; }
        [Required]
        [MinLength(3)]
        public string Password { get; set; }

        [Required(ErrorMessage = "You must provide a phone number")]
        [RegularExpression(@"((01|09|03|07|08|05)+([0-9]{8})\b)", ErrorMessage = "Not a valid phone number")]
        
        public string Phone { get; set; }

        [Required(ErrorMessage = "Please fill your address")]
        public string Address { get; set; }
        public bool? Status { get; set; }

        public virtual TblRole IdRoleNavigation { get; set; }
        public virtual ICollection<TblOrder> TblOrder { get; set; }
        public virtual ICollection<TblProduct> TblProduct { get; set; }
        public virtual ICollection<TblTempCart> TblTempCart { get; set; }
    }
}
