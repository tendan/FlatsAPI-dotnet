using Microsoft.Extensions.DependencyInjection;

namespace FlatsAPI.Settings.Roles;

public static class RoleExtensions
{
    public static IServiceCollection AddRolesSingletons(this IServiceCollection services)
    {
        services.AddSingleton<IRole, AdminRole>();
        services.AddSingleton<IRole, LandlordRole>();
        services.AddSingleton<IRole, TenantRole>();

        return services;
    }
}
