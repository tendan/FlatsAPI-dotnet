using FlatsAPI.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlatsAPI.Models.Validators
{
    public class UpdateAccountDtoValidator : AbstractValidator<UpdateAccountDto>
    {
        public UpdateAccountDtoValidator(FlatsDbContext dbContext)
        {
            RuleFor(a => a.Email)
                .Cascade(CascadeMode.Stop)
                .EmailAddress();

            RuleFor(a => a.Username)
                .Cascade(CascadeMode.Stop)
                .MinimumLength(6)
                .MaximumLength(30);

            RuleFor(a => a.FirstName)
                .Cascade(CascadeMode.Stop)
                .Length(2, 50);

            RuleFor(a => a.LastName)
                .Cascade(CascadeMode.Stop)
                .Length(2, 50);

            RuleFor(a => a.Password)
                .Cascade(CascadeMode.Stop)
                .Length(8, 50);

            RuleFor(a => a.ConfirmPassword)
                .Equal(e => e.Password);

            RuleFor(a => a.PhoneNumber)
                .PhoneNumber();

            RuleFor(a => a.Email)
                .Custom((value, context) =>
                {
                    var emailInUse = dbContext.Accounts.Any(a => a.Email == value);
                    if (emailInUse)
                        context.AddFailure("Email", "That email is taken");
                });

            RuleFor(a => a.Username)
                .Custom((value, context) =>
                {
                    var usernameInUse = dbContext.Accounts.Any(a => a.Username == value);
                    if (usernameInUse)
                        context.AddFailure("Username", "That username is taken");
                });
        }
        

    }
}
