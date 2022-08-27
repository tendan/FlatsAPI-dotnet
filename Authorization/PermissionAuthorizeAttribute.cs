using Microsoft.AspNetCore.Authorization;

namespace FlatsAPI.Authorization;

/// <summary>
/// Authorizes user by required permission
/// </summary>
public class PermissionAuthorizeAttribute : AuthorizeAttribute
{
    const string POLICY_PREFIX = "Permission";
    private string _permission;

    /// <summary>
    /// Describe what permission is required for specified endpoint
    /// 
    /// It is recommended to use enumerable type in Settings\Permissions
    /// </summary>
    /// <param name="permission"></param>
    public PermissionAuthorizeAttribute(string permission) => Permission = permission;

    public string Permission
    {
        get
        {
            return _permission;
        }
        set
        {
            _permission = value;
            Policy = $"{POLICY_PREFIX}:{_permission}";
        }
    }
}
