using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace GymApp.Validation
{
    public class FutureDate : ValidationAttribute
    {
        public FutureDate() 
        {
            ErrorMessage = "Date must be in the future";
        }

        public override bool IsValid(object value)
        {
            if (value == null) return false;

            DateTime date = (DateTime)value;
            return date > DateTime.Now;

        }
    }
}