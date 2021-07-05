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
        private static ICollection<string> _permissions = new List<string>();

        static AdminRole()
        {
            var permissionsFields = new List<IPermissions>()
            {
                new AccountPermissions(),
                new BlockOfFlatsPermissions(),
                new FlatPermissions()
            };

            var fields = new List<FieldInfo>();

            foreach (var type in permissionsFields)
            {
                var properties = type.GetType().GetFields();
                fields.AddRange(properties);
            }

            foreach (var property in fields)
            {
                var permissionContext = scope.ServiceProvider.GetService<IPermissionContext>();
                _permissions = permissionContext.GetAllModulesPermissions();
            }
        }
        public static string Name { get; } = "Admin";

        public static ICollection<string> Permissions { get; } = _permissions;
    }
}
