using FlatsAPI.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace FlatsAPI.Services;

public interface IUserContextService
{
    ClaimsPrincipal User { get; }
    int? GetUserId { get; }
    void AuthorizeAccess(int? accountId, string permission);
}
public class UserContextService : IUserContextService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IPermissionContext _permissionContext;

    public UserContextService(IHttpContextAccessor httpContextAccessor, IPermissionContext permissionContext)
    {
        _httpContextAccessor = httpContextAccessor;
        _permissionContext = permissionContext;
    }

    public ClaimsPrincipal User => _httpContextAccessor.HttpContext?.User;

    public int? GetUserId => User is null
        ? null
        : (int?)int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);

    public void AuthorizeAccess(int? accountId, string permission)
    {
        var userId = (int)GetUserId;

        var isAllowedToDeleteOthersAccounts = _permissionContext.IsPermittedToPerformAction(permission, userId);

        if (accountId != userId && !isAllowedToDeleteOthersAccounts)
            throw new ForbiddenException("You are not permitted to perform this action");
    }
}
