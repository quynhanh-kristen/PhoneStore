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
}
