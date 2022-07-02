using System.Security.Claims;
using ICPC_WebSite_Backend.Data.Models;
using Microsoft.AspNetCore.Authorization;

namespace ICPC_WebSite_Backend.Security;

public class AuthorizationHandler : AuthorizationHandler<AuthorizationRequirement, ClaimResource>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthorizationRequirement requirement, ClaimResource resource)
    {
        if (!context.User.HasClaim(claim => claim.Type == resource.ClaimName))
        {
            return Task.CompletedTask;
        }

        var communityId = context.User.FindFirst(resource.ClaimName)?.Value;
        if (Convert.ToInt32(communityId) == resource.ResourceId)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}