using PhoneStore.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneStore.Validation
{
    public class AdditionalRequired : RequiredAttribute
    {
       public AdditionalRequired()
        {
            ErrorMessage = "{0} is required";
        }

    }

    public class EmailUserUniqueAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(
            object value, ValidationContext validationContext)
        {
            var _context = (PhoneManagementContext)validationContext.GetService(typeof(PhoneManagementContext));
            var entity = _context.TblUser.SingleOrDefault(e => e.Phone == value.ToString());

            if (entity != null)
            {
                return new ValidationResult(GetErrorMessage(value.ToString()));
            }
            return ValidationResult.Success;
        }

        public string GetErrorMessage(string phone)
        {
            return $"Email {phone} is already in use.";
        }
    }
}
