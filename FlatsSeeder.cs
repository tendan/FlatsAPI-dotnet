using FlatsAPI.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlatsAPI.Settings;
using FlatsAPI.Settings.Permissions;
using System.Reflection;
using FlatsAPI.Settings.Roles;

namespace FlatsAPI
{
    public class FlatsSeeder
    {
        private readonly FlatsDbContext _dbContext;
        private readonly IPasswordHasher<Account> _passwordHasher;
        private readonly IPermissionContext _permissionContext;

        public FlatsSeeder(FlatsDbContext dbContext, IPasswordHasher<Account> passwordHasher, IPermissionContext permissionContext)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
            _permissionContext = permissionContext;
        }
        public void Seed()
        {
            if (_dbContext.Database.CanConnect())
            {
                /*if (_dbContext.Roles.Any())
                {*/
                /*_dbContext.Roles.RemoveRange(_dbContext.Roles.ToList());
                _dbContext.Accounts.RemoveRange(_dbContext.Accounts.ToList());
                _dbContext.Permissions.RemoveRange(_dbContext.Permissions.ToList());
                _dbContext.SaveChanges();*/
                /*}*/
                if (!_dbContext.Permissions.Any())
                {
                    var permissions = GetPermissions();
                    _dbContext.Permissions.AddRange(permissions);
                    _dbContext.SaveChanges();
                }
                if (!_dbContext.Roles.Any())
                {
                    var roles = GetRoles();
                    _dbContext.Roles.AddRange(roles);
                    _dbContext.SaveChanges();
                }
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
                    RoleId = 2,
                },
                new Account()
                {
                    Email = "hanka.skakanka5@outlook.com",
                    Username = "h.skakanka",
                    FirstName = "Hania",
                    LastName = "Brzęczyszczykiewicz",
                    RoleId = 1,
                },
                new Account()
                {
                    Email = "admin123@test.com",
                    Username = "admin",
                    FirstName = "Franek",
                    LastName = "Administrator",
                    RoleId = 3
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
                },
                new BlockOfFlats()
                {
                    Address = "Ogrodowa 13",
                    PostalCode = "31-512",
                    Floors = 9,
                    Margin = 62,
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
                }
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
