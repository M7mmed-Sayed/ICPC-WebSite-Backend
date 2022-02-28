using Microsoft.AspNetCore.Identity;
namespace ICPC_WebSite_Backend.Models
{
    public class User:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? SecondaryEmail  { get; set; }
        public string? FaceBookProfile { get; set; }
    }
}
