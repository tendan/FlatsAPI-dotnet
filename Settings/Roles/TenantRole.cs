using FlatsAPI.Settings.Permissions;
using System.Collections.Generic;

namespace FlatsAPI.Settings.Roles;

public class TenantRole : IRole
{
    public static string Name { get; } = "Tenant";
    public static ICollection<string> Permissions { get; } = new List<string>()
    {
        BlockOfFlatsPermissions.Read,
        FlatPermissions.Read,
        InvoicePermissions.Read,
    };
}
