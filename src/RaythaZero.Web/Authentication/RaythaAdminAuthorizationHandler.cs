using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using RaythaZero.Application.Common.Security;
using RaythaZero.Domain.Entities;

namespace Raytha.Web.Authentication;

public class IsAdminRequirement : IAuthorizationRequirement
{
}

public class ManageUsersRequirement : IAuthorizationRequirement
{
}

public class ManageSystemSettingsRequirement : IAuthorizationRequirement
{
}

public class ManageAdministratorsRequirement : IAuthorizationRequirement
{
}

public class ManageAuditLogsRequirement : IAuthorizationRequirement
{
}


public class RaythaAdminAuthorizationHandler : IAuthorizationHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor = null;

    public RaythaAdminAuthorizationHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Task HandleAsync(AuthorizationHandlerContext context)
    {
        if (context.User == null || !context.User.Identity.IsAuthenticated || !IsAdmin(context.User))
        {
            return Task.CompletedTask;
        }

        var systemPermissionsClaims = context.User.Claims.Where(p => p.Type == RaythaClaimTypes.SystemPermissions).Select(p => p.Value).ToArray();
        var contentTypePermissionsClaims = context.User.Claims.Where(c => c.Type == RaythaClaimTypes.ContentTypePermissions).Select(p => p.Value).ToArray();

        var pendingRequirements = context.PendingRequirements.ToList();

        foreach (var requirement in pendingRequirements)
        {
            if (requirement is IsAdminRequirement)
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            if (requirement is ManageUsersRequirement)
            {
                if (systemPermissionsClaims.Contains(BuiltInSystemPermission.MANAGE_USERS_PERMISSION))
                {
                    context.Succeed(requirement);
                }
            }
            else if (requirement is ManageSystemSettingsRequirement)
            {
                if (systemPermissionsClaims.Contains(BuiltInSystemPermission.MANAGE_SYSTEM_SETTINGS_PERMISSION))
                {
                    context.Succeed(requirement);
                }
            }
            else if (requirement is ManageAuditLogsRequirement)
            {
                if (systemPermissionsClaims.Contains(BuiltInSystemPermission.MANAGE_AUDIT_LOGS_PERMISSION))
                {
                    context.Succeed(requirement);
                }
            }
            else if (requirement is ManageAdministratorsRequirement)
            {
                if (systemPermissionsClaims.Contains(BuiltInSystemPermission.MANAGE_ADMINISTRATORS_PERMISSION))
                {
                    context.Succeed(requirement);
                }
            }
        }

        return Task.CompletedTask;
    }

    private static bool IsAdmin(ClaimsPrincipal user)
    {
        var isAdminClaim = user.Claims.FirstOrDefault(c => c.Type == RaythaClaimTypes.IsAdmin);
        if (isAdminClaim == null)
        {
            return false;
        }
        return Convert.ToBoolean(isAdminClaim.Value);
    }
}