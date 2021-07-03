using FlatsAPI.Entities;
using FlatsAPI.Exceptions;
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
        Permission GetPermissionFromDb(string permissionName);
        ICollection<Permission> GetAllPermissionsFromDb();
        ICollection<string> GetAllModulesPermissions();
    }
    public class PermissionContext : IPermissionContext
    {
        private readonly FlatsDbContext _dbContext;

        public PermissionContext(FlatsDbContext dbContext)
        {
            _dbContext = dbContext;
        }
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
        public Permission GetPermissionFromDb(string permissionName)
        {
            var permission = _dbContext.Permissions.FirstOrDefault(p => p.Name == permissionName);

            if (permission is null)
                throw new NotFoundException("Permission not found");

            return permission;
        }
        public ICollection<Permission> GetAllPermissionsFromDb()
        {
            var permissions = _dbContext.Permissions.ToList();

            return permissions;
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
