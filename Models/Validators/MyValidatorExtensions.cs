using FlatsAPI.Models.Validators.PropertyValidators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace FlatsAPI.Models.Validators;

public static class MyValidatorExtensions
{
    public static IRuleBuilderOptions<T, TElement> PhoneNumber<T, TElement>(this IRuleBuilder<T, TElement> ruleBuilder)
    {
        return ruleBuilder.SetValidator(new PhoneNumberValidator<T, TElement>());
    }

    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<CreateAccountDto>, CreateAccountDtoValidator>();
        services.AddScoped<IValidator<UpdateAccountDto>, UpdateAccountDtoValidator>();

        return services;
    }
}
