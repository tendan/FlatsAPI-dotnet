﻿using FlatsAPI.Entities;
using FlatsAPI.Exceptions;
using FlatsAPI.Settings.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace FlatsAPI.Services
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
                permissions.Add(property.GetValue(null).ToString());
            }

            return permissions;
        }
    }
}