using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string Name { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]+$")]
        public decimal Cost { get; set; }
        [Required]
        [Range(1, 100)]
        public int Quantity { get; set; }

       //[RegularExpression(@"\.(jpg|jpeg|png)$",
       //     ErrorMessage = "The file must end with png, jpg, or jpeg")]
        public string Image { get; set; }
        public string Description { get; set; }

    
        public string Configuration { get; set; }

        [Range(1,5)]
        public int? Rating { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UserCreatedId { get; set; }

        public virtual TblCategory IdCtgPhoneNavigation { get; set; }
        public virtual TblUser UserCreated { get; set; }
        public virtual ICollection<TblOrderDetail> TblOrderDetail { get; set; }
    }
}
