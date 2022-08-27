using FlatsAPI.Entities;
using FlatsAPI.Exceptions;
using FlatsAPI.Settings.Permissions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FlatsAPI.Services;

public interface IPermissionContext
{
    ICollection<string> GeneratePermissionsForModule(string module);
    Permission GetPermissionFromDb(string permissionName);
    ICollection<Permission> GetAllPermissionsFromDb();
    ICollection<string> GetAllModulesPermissions();
    ICollection<Permission> GetAllAccountPermissionsById(int accountId);
    Role GetAccountRoleById(int accountId);
    bool IsPermittedToPerformAction(string permission, int accountId);
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

        var permissionsFields = new List<IPermissions>()
        {
            new AccountPermissions(),
            new BlockOfFlatsPermissions(),
            new FlatPermissions(),
            new InvoicePermissions(),
        };

        var fields = new List<FieldInfo>();

        foreach (var type in permissionsFields)
        {
            var properties = type.GetType().GetFields();
            fields.AddRange(properties);
        }

        foreach (var property in fields)
        {
            permissions.Add(property.GetValue(null).ToString());
        }

        return permissions;
    }

    public ICollection<Permission> GetAllAccountPermissionsById(int accountId)
    {
        var account = _dbContext.Accounts
            .Include(a => a.Role)
            .Include(a => a.Role.Permissions)
            .FirstOrDefault(a => a.Id == accountId);

        return account.Role.Permissions;
    }

    public Role GetAccountRoleById(int accountId)
    {
        var account = _dbContext.Accounts.Include(a => a.Role).FirstOrDefault(a => a.Id == accountId);

        return account.Role;
    }

    public bool IsPermittedToPerformAction(string permission, int accountId)
    {
        var permissions = GetAllAccountPermissionsById(accountId);

        return permissions.FirstOrDefault(p => p.Name == permission) is not null;
    }
}
