using FlatsAPI.Settings.Permissions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace FlatsAPI.Settings.Roles
{
    public class AdminRole : IRole
    {
        private static ICollection<string> _permissions;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public AdminRole(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var permissionContext = scope.ServiceProvider.GetService<IPermissionContext>();
                _permissions = permissionContext.GetAllModulesPermissions();
            }
        }
        public static string Name { get; } = "Admin";

        public static ICollection<string> Permissions { get; } = _permissions;
    }
}
