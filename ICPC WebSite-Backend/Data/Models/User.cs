using Microsoft.AspNetCore.Identity;
namespace ICPC_WebSite_Backend.Data.Models
{
    public class User:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? SecondaryEmail  { get; set; }
        public string? FaceBookProfile { get; set; }
        public ICollection<CommunityMember> CommunityRoles{ get; set; }
    }
}
