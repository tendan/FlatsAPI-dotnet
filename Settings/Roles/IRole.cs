using System.Collections.Generic;

namespace FlatsAPI.Settings.Roles;

interface IRole
{
    public static string Name { get; }
    public static ICollection<string> Permissions { get; }
}
