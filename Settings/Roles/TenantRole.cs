using FlatsAPI.Entities;
using FlatsAPI.Settings.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlatsAPI.Settings.Roles
{
    public class TenantRole : IRole
    {
        public static string Name { get; } = "Tenant";
        public static ICollection<string> Permissions { get; } = new List<string>()
        {
            BlockOfFlatsPermissions.Read,
            FlatPermissions.Read
        };
    }
}
