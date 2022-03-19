using ICPC_WebSite_Backend.Data;
using ICPC_WebSite_Backend.Models;
using ICPC_WebSite_Backend.Repository;
using ICPC_WebSite_Backend.Utility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ICPC_WebSite_Backend.Configurations
{
    public static class ServiceExtensions
    {
        public static async Task CreateRoles(this IServiceProvider Services) {
            //initializing custom roles 
            var RoleManager = Services.GetRequiredService<RoleManager<IdentityRole>>();
            var roleNames = RolesList.Roles;
            foreach (var roleName in roleNames) {
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist) {
                    IdentityResult roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
            return;
        }
    }
}
