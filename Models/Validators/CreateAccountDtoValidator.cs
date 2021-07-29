using FlatsAPI.Entities;
using FlatsAPI.Settings.Roles;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FlatsAPI.Models.Validators
{
    public class CreateAccountDtoValidator : AbstractValidator<CreateAccountDto>
    {
        public CreateAccountDtoValidator(FlatsDbContext dbContext)
        {
            RuleFor(a => a.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(a => a.Username)
                .NotEmpty()
                .MinimumLength(6)
                .MaximumLength(30);

            RuleFor(a => a.FirstName)
                .NotEmpty();

            RuleFor(a => a.LastName)
                .NotEmpty();

            RuleFor(a => a.Password)
                .NotEmpty()
                .MinimumLength(8)
                .MaximumLength(50);

            RuleFor(a => a.ConfirmPassword)
                .Equal(e => e.Password);

            RuleFor(a => a.PhoneNumber)
                .PhoneNumber();

            RuleFor(a => a.RoleId)
                .NotEqual(dbContext.Roles.FirstOrDefault(r => r.Name == AdminRole.Name).Id)
                .WithMessage("You cannot create Admin account");

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
