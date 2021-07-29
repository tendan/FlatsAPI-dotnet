using FlatsAPI.Models.Validators.PropertyValidators;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlatsAPI.Models.Validators
{
    public static class MyValidatorExtensions
    {
        public static IRuleBuilderOptions<T, TElement> PhoneNumber<T, TElement>(this IRuleBuilder<T, TElement> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new PhoneNumberValidator<T, TElement>());
        }
    }
}
