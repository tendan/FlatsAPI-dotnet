using FlatsAPI.Settings.Permissions;
using System.Collections.Generic;

namespace FlatsAPI.Settings.Roles;

public class LandlordRole : IRole
{
    public static string Name { get; } = "Landlord";

    public static ICollection<string> Permissions { get; } = new List<string>()
    {
        BlockOfFlatsPermissions.Create,
        BlockOfFlatsPermissions.Read,
        BlockOfFlatsPermissions.Update,
        BlockOfFlatsPermissions.Delete,
        FlatPermissions.Create,
        FlatPermissions.Read,
        FlatPermissions.Update,
        FlatPermissions.Delete,
        FlatPermissions.ApplyTenant,
        InvoicePermissions.Read,
    };
}
