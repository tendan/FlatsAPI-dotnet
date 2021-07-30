using FluentValidation;
using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FlatsAPI.Models.Validators.PropertyValidators
{
    public class PhoneNumberValidator<T, TElement> : PropertyValidator<T, TElement>
    {
        public override string Name => "PhoneNumberValidator";

        public PhoneNumberValidator() { }

        public override bool IsValid(ValidationContext<T> context, TElement value)
        {
            var pattern
                = "^(\\+\\d{1,3}( )?)?((\\(\\d{3}\\))|\\d{3})[- .]?\\d{3}[- .]?\\d{4}$"
                + "|^(\\+\\d{1,3}( )?)?(\\d{3}[ ]?){2}\\d{3}$"
                + "|^(\\+\\d{1,3}( )?)?(\\d{3}[ ]?)(\\d{2}[ ]?){2}\\d{2}$";

            var phoneNumberRegex = new Regex(pattern);

            if (value is not null)
            {
                if (!phoneNumberRegex.IsMatch(value.ToString()))
                {
                    return false;
                }
            }
            return true;
        }
        protected override string GetDefaultMessageTemplate(string errorCode)
        => "Must be a phone number";
    }
}
