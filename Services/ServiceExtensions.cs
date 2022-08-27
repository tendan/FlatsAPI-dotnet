using FlatsAPI.Services.Scheduled;
using Microsoft.Extensions.DependencyInjection;

namespace FlatsAPI.Services;

public static class ServiceExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddTransient<IPermissionContext, PermissionContext>();
        services.AddScoped<IUserContextService, UserContextService>();
        services.AddScoped<IBlockOfFlatsService, BlockOfFlatsService>();
        services.AddScoped<IFlatService, FlatService>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IInvoiceService, InvoiceService>();
        services.AddTransient<IRentService, RentService>();

        return services;
    }

    public static IServiceCollection AddHostedServices(this IServiceCollection services)
    {
        services.AddHostedService<RentHostedService>();

        return services;
    }
}
