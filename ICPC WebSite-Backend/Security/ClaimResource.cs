using Microsoft.AspNetCore.Mvc;
namespace ICPC_WebSite_Backend.Security;

public class ClaimResource:ControllerBase
{
    public string ClaimName { get; set; }
    public string ResourceId { get; set; }

    public ClaimResource(string claimName, string resourceId)
    {
        ClaimName = claimName;
        ResourceId = resourceId;
    }
    public ClaimResource(string claimName, int resourceId)
    {
        ClaimName = claimName;
        ResourceId = resourceId.ToString();
    }
}