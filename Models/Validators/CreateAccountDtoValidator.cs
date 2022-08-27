using FlatsAPI.Entities;
using FlatsAPI.Settings.Roles;
using FluentValidation;
using System.Linq;

namespace FlatsAPI.Models.Validators;

public class CreateAccountDtoValidator : AbstractValidator<CreateAccountDto>
{
    public CreateAccountDtoValidator(FlatsDbContext dbContext)
    {
        RuleFor(a => a.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .EmailAddress();

        RuleFor(a => a.Username)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .Length(6, 30);

        RuleFor(a => a.FirstName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .Length(2, 50);

        RuleFor(a => a.LastName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .Length(2, 50);

        RuleFor(a => a.Password)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .Length(8, 50);

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
