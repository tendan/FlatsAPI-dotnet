using FlatsAPI.Settings.Permissions;
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
        AdminRole()
        {
            FieldInfo[] accountPermissionsProperties = typeof(AccountPermissions).GetFields();
            FieldInfo[] blockOfFlatsPermissionsProperties = typeof(BlockOfFlatsPermissions).GetFields();
            FieldInfo[] flatPermissionsProperties = typeof(FlatPermissions).GetFields();

            var fields = new List<FieldInfo>();

            fields.AddRange(accountPermissionsProperties);
            fields.AddRange(blockOfFlatsPermissionsProperties);
            fields.AddRange(flatPermissionsProperties);

            foreach (var property in fields)
            {
                _permissions.Add(property.GetValue(null).ToString());
            }
        }
        public static string Name { get; } = "Admin";

        public static ICollection<string> Permissions { get; } = _permissions;
    }
}
