using ICPC_WebSite_Backend.Data.Models;
using Microsoft.AspNetCore.Authorization;
using ICPC_WebSite_Backend.Data.Models.DTO;
using ICPC_WebSite_Backend.Repository;
using ICPC_WebSite_Backend.Security;
using ICPC_WebSite_Backend.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace ICPC_WebSite_Backend.Security;

public class ClaimResource:ControllerBase
{
    public string ClaimName { get; set; }
    public int ResourceId { get; set; }

    public ClaimResource(string claimName, int resourceId)
    {
        ClaimName = claimName;
        ResourceId = resourceId;
    }
}