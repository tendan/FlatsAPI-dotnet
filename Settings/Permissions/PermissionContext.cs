using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace FlatsAPI.Settings.Permissions
{
    public interface IPermissionContext
    {
        ICollection<string> GeneratePermissionsForModule(string module);
        ICollection<string> GetAllModulesPermissions();
    }
    public class PermissionContext : IPermissionContext
    {
        public ICollection<string> GeneratePermissionsForModule(string module)
        {
            return new List<string>()
            {
                $"{module}.Create",
                $"{module}.Read",
                $"{module}.Update",
                $"{module}.Delete"
            };
        }
        public ICollection<string> GetAllModulesPermissions()
        {
            var permissions = new List<string>();
            FieldInfo[] accountPermissionsProperties = typeof(AccountPermissions).GetFields();
            FieldInfo[] blockOfFlatsPermissionsProperties = typeof(BlockOfFlatsPermissions).GetFields();
            FieldInfo[] flatPermissionsProperties = typeof(FlatPermissions).GetFields();

            var fields = new List<FieldInfo>();

            fields.AddRange(accountPermissionsProperties);
            fields.AddRange(blockOfFlatsPermissionsProperties);
            fields.AddRange(flatPermissionsProperties);

            foreach (var property in fields)
            {
                permissions.Add(property.GetValue(null).ToString());
            }

            return permissions;
        }
    }
}
