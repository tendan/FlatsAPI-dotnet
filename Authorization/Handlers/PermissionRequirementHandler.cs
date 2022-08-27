using FlatsAPI.Entities;
using FlatsAPI.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FlatsAPI.Authorization.Handlers;

public class PermissionRequirementHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly FlatsDbContext _dbContext;

    public PermissionRequirementHandler(
        FlatsDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        if (!context.User.HasClaim(c => c.Type == ClaimTypes.NameIdentifier))
        {
            throw new ForbiddenException("You are not permitted to perform this action");
        }

        var accountId = int.Parse(context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);

        var account = _dbContext.Accounts.Include(a => a.Role).FirstOrDefault(a => a.Id == accountId);

        var accountRole = account.Role;

        var role = _dbContext.Roles.Include(r => r.Permissions).FirstOrDefault(r => r == accountRole);

        var rolePermissions = role?.Permissions;

        if (!rolePermissions.Any())
        {
            throw new ForbiddenException("You are not permitted to perform this action");
        }

        var requiredPermissionInPermissions = rolePermissions.FirstOrDefault(p => p.Name == requirement.Permission);

        if (requiredPermissionInPermissions is not null)
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        return Task.CompletedTask;
    }
}
