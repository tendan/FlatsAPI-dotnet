using FlatsAPI.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using FlatsAPI.Settings.Roles;
using FlatsAPI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using FlatsAPI.Models.Interfaces;

namespace FlatsAPI
{
    public class FlatsSeeder
    {
        private readonly FlatsDbContext _dbContext;
        private readonly IPasswordHasher<Account> _passwordHasher;
        private readonly IPermissionContext _permissionContext;
        private readonly ILogger<FlatsSeeder> _logger;

        public FlatsSeeder(FlatsDbContext dbContext, 
            IPasswordHasher<Account> passwordHasher, 
            IPermissionContext permissionContext,
            ILogger<FlatsSeeder> logger)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
            _permissionContext = permissionContext;
            _logger = logger;
        }
        public void Seed()
        {
            if (_dbContext.Database.CanConnect())
            {
                SetPermissions();
                SetRoles();
                if (!_dbContext.Accounts.Any())
                {
                    var accounts = GetAccounts();
                    _dbContext.Accounts.AddRange(accounts);
                    _dbContext.SaveChanges();
                }
                if (!_dbContext.BlockOfFlats.Any())
                {
                    var blockOfFlats = GetBlockOfFlats();
                    _dbContext.BlockOfFlats.AddRange(blockOfFlats);
                    _dbContext.SaveChanges();
                }
                if (!_dbContext.Flats.Any())
                {
                    var flats = GetFlats();
                    _dbContext.Flats.AddRange(flats);
                    _dbContext.SaveChanges();
                }
            }
        }

        private ICollection<Account> GetAccounts()
        {
            var accounts = new List<Account>()
            {
                new Account()
                {
                    Email = "tomasz.dran14@gmail.com",
                    Username = "tomasz.dran",
                    FirstName = "Tomasz",
                    LastName = "Drań",
                    Role = _dbContext.Roles.FirstOrDefault(r => r.Name == LandlordRole.Name)
                },
                new Account()
                {
                    Email = "hanka.skakanka5@outlook.com",
                    Username = "h.skakanka",
                    FirstName = "Hania",
                    LastName = "Brzęczyszczykiewicz",
                    Role = _dbContext.Roles.FirstOrDefault(r => r.Name == TenantRole.Name)
                },
                new Account()
                {
                    Email = "admin123@test.com",
                    Username = "admin",
                    FirstName = "Franek",
                    LastName = "Administrator",
                    Role = _dbContext.Roles.FirstOrDefault(r => r.Name == AdminRole.Name)
                }
            };
            accounts.ForEach((a) => a.Password = _passwordHasher.HashPassword(a, "test12345"));

            return accounts;
        }

        private ICollection<BlockOfFlats> GetBlockOfFlats()
        {
            var blocks = new List<BlockOfFlats>()
            {
                new BlockOfFlats()
                {
                    Address = "Kwiatowa 15/23",
                    PostalCode = "42-360",
                    Floors = 3,
                    Margin = 25,
                    Owner = _dbContext.Accounts.FirstOrDefault(a => a.Username == "tomasz.dran"),
                    Price = 450340
                },
                new BlockOfFlats()
                {
                    Address = "Ogrodowa 13",
                    PostalCode = "31-512",
                    Floors = 9,
                    Margin = 62,
                    Price = 325500
                }
            };
            return blocks;
        }

        private ICollection<Flat> GetFlats()
        {
            var flats = new List<Flat>()
            {
                new Flat()
                {
                    Area = 52,
                    Number = 5,
                    NumberOfRooms = 4,
                    Floor = 3,
                    BlockOfFlatsId = 1,
                    PriceWhenBought = 123500,
                    Owner = _dbContext.Accounts.FirstOrDefault(a => a.Username == "admin")
                },
                new Flat()
                {
                    Area = 63,
                    Number = 3,
                    NumberOfRooms = 6,
                    Floor = 2,
                    BlockOfFlatsId = 1,
                    PriceWhenBought = 254260,
                    PricePerMeterSquaredWhenRented = 250,
                },
                new Flat()
                {
                    Area = 42,
                    Number = 1,
                    NumberOfRooms = 4,
                    Floor = 6,
                    BlockOfFlatsId = 2,
                    PriceWhenBought = 42000,
                    Owner = _dbContext.Accounts.FirstOrDefault(a => a.Username == "tomasz.dran")
                }
            };
            return flats;
        }

        private ICollection<Role> GetRoles()
        {
            var roles = new List<Role>()
            {
                new Role()
                {
                    Name = TenantRole.Name,
                    Permissions = GetTenantPermissions()
                },
                new Role()
                {
                    Name = LandlordRole.Name,
                    Permissions = GetLandlordPermissions()
                },
                new Role()
                {
                    Name = AdminRole.Name,
                    Permissions = GetAdminPermissions()
                },
            };
            return roles;
        }

        private ICollection<Permission> GetPermissions()
        {
            var permissions = new List<Permission>();

            var permissionStrings = _permissionContext.GetAllModulesPermissions();
            
            foreach (var permission in permissionStrings)
            {
                permissions.Add(new Permission() { Name = permission });
            }

            return permissions;
        }

        private void SetPermissions()
        {
            var permissions = GetPermissions();

            var permissionsInDb = _dbContext.Permissions.ToList();

            AddEntities(permissions, permissionsInDb);
            RemoveEntities(permissions, permissionsInDb);

            _logger.LogInformation("Saved permissions to DB");
            _dbContext.SaveChanges();
        }

        private void AddEntities<T>(IEnumerable<T> entitiesFromInstance, IEnumerable<T> entitiesInDb) where T : INameable, new()
        {
            _logger.LogInformation($"Check for {typeof(T).GetTypeInfo().Name} additional changes");
            foreach (var entityFromInstance in entitiesFromInstance)
            {
                if (!entitiesInDb.Any(r => r.Name == entityFromInstance.Name))
                {
                    _logger.LogInformation($"Role {entityFromInstance.Name} added to DB");
                    _dbContext.Add(entityFromInstance);
                    continue;
                }
                if (entityFromInstance is Permission) continue;
                _logger.LogInformation($"Check for {typeof(T).Name}'s permissions additional changes");
                // If it is a role, do a permission add to role everytime it should happen
                foreach (var permission in (entityFromInstance as Role).Permissions.ToList())
                {
                    var roleFromDb = entitiesInDb.FirstOrDefault(r => r.Name == entityFromInstance.Name) as Role;

                    if (!roleFromDb.Permissions.Any(p => p.Name == permission.Name))
                    {
                        _logger.LogInformation($"Permission {permission.Name} added to {entityFromInstance.Name} role");
                        roleFromDb.Permissions.Add(permission);
                        _dbContext.Update(roleFromDb);
                    }
                }
            }
        }

        private void RemoveEntities<T>(IEnumerable<T> entitiesFromInstance, IEnumerable<T> entitiesInDb) where T : INameable, new()
        {
            _logger.LogInformation($"Check for {typeof(T).GetTypeInfo().Name} removal changes");
            foreach (var entityInDb in entitiesInDb.ToList())
            {
                if (!entitiesFromInstance.Any(p => p.Name == entityInDb.Name))
                {
                    _logger.LogInformation($"{entityInDb.GetType()} {entityInDb.Name} removed from DB");
                    _dbContext.Remove(entityInDb);
                    continue;
                }
                if (entityInDb is Permission) continue;
                _logger.LogInformation($"Check for {typeof(T).Name} permission removal changes");
                // If it is a role, do a permission removal from roles everytime it should happen
                foreach (var subEntityInDb in (entityInDb as Role).Permissions.ToList())
                {
                    var entityFromSingleton = (entitiesFromInstance as IEnumerable<Role>).FirstOrDefault(r => r.Name == entityInDb.Name);

                    if (!entityFromSingleton.Permissions.Any(p => p.Name == subEntityInDb.Name))
                    {
                        _logger.LogInformation($"Permission {subEntityInDb.Name} removed from {entityInDb.Name} role");
                        _dbContext.Remove(entityInDb);
                    }
                }
            }
        }

        private void SetRoles()
        {
            var roles = GetRoles();

            var rolesInDb = _dbContext.Roles.Include(r => r.Permissions).ToList();

            AddEntities(roles, rolesInDb);
            RemoveEntities(roles, rolesInDb);

            _logger.LogInformation("Saved roles to DB");
            _dbContext.SaveChanges();
        }

        [Obsolete]
        private void RemovePermissions(ICollection<Permission> permissionsFromInstance, List<Permission> permissionsInDb)
        {
            foreach (var permissionInDb in permissionsInDb)
            {
                if (!permissionsFromInstance.Any(p => p.Name == permissionInDb.Name))
                {
                    _logger.LogInformation($"Permission {permissionInDb.Name} removed from DB");
                    _dbContext.Permissions.Remove(permissionInDb);
                }
            }
        }

        [Obsolete]
        private void AddPermissions(ICollection<Permission> permissionsFromInstance, List<Permission> permissionsInDb)
        {
            foreach (var permission in permissionsFromInstance)
            {
                if (!permissionsInDb.Any(p => p.Name == permission.Name))
                {
                    _logger.LogInformation($"Permission {permission.Name} added to DB");
                    _dbContext.Permissions.Add(permission);
                }
            }
        }

        [Obsolete]
        private void RemoveRoles(ICollection<Role> rolesFromInstance, List<Role> rolesInDb)
        {
            foreach (var roleinDb in rolesInDb)
            {
                if (!rolesFromInstance.Any(r => r.Name == roleinDb.Name))
                {
                    _logger.LogInformation($"Role {roleinDb.Name} removed from DB");
                    _dbContext.Remove(roleinDb);
                    continue;
                }
                foreach (var permission in roleinDb.Permissions.ToList())
                {
                    var roleFromSingleton = rolesFromInstance.FirstOrDefault(r => r.Name == roleinDb.Name);

                    if (!roleFromSingleton.Permissions.Any(p => p.Name == permission.Name))
                    {
                        _logger.LogInformation($"Permission {permission.Name} removed from {roleinDb.Name} role");
                        _dbContext.Remove(roleinDb);
                    }
                }
            }
        }

        [Obsolete]
        private void AddRoles(ICollection<Role> rolesFromInstance, List<Role> rolesInDb)
        {
            foreach (var role in rolesFromInstance)
            {
                if (!rolesInDb.Any(r => r.Name == role.Name))
                {
                    _logger.LogInformation($"Role {role.Name} added to DB");
                    _dbContext.Add(role);
                    continue;
                }
                foreach (var permission in role.Permissions.ToList())
                {
                    var roleFromDb = rolesInDb.FirstOrDefault(r => r.Name == role.Name);

                    if (!roleFromDb.Permissions.Any(p => p.Name == permission.Name))
                    {
                        _logger.LogInformation($"Permission {permission.Name} added to {role.Name} role");
                        roleFromDb.Permissions.Add(permission);
                        _dbContext.Update(roleFromDb);
                    }
                }
            }
        }

        private ICollection<Permission> GetTenantPermissions()
        {
            var permissions = new List<Permission>();

            foreach (var permission in TenantRole.Permissions)
                permissions.Add(_permissionContext.GetPermissionFromDb(permission));

            return permissions;
        }

        private ICollection<Permission> GetAdminPermissions()
        {
            var permissions = new List<Permission>();

            foreach (var permission in AdminRole.Permissions)
                permissions.Add(_permissionContext.GetPermissionFromDb(permission));

            return permissions;
        }

        private ICollection<Permission> GetLandlordPermissions()
        {
            var permissions = new List<Permission>();

            foreach (var permission in LandlordRole.Permissions)
                permissions.Add(_permissionContext.GetPermissionFromDb(permission));

            return permissions;
        }
    }
}
