using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using UtilityLibrary.Utility;

namespace ICPC_WebSite_Backend.Security;

public class AuthorizationHandler : AuthorizationHandler<AuthorizationRequirement, ClaimResource>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthorizationRequirement requirement, ClaimResource resource)
    {
        if (context.User.FindAll(ClaimTypes.Role).FirstOrDefault(claim => claim.Value == RolesList.Administrator) != null)
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        if (!context.User.HasClaim(claim => claim.Type == resource.ClaimName))
        {
            return Task.CompletedTask;
        }

        var communityId = context.User.FindFirst(resource.ClaimName)?.Value;
        if (communityId == resource.ResourceId)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}