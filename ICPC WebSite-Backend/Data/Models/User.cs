using Microsoft.AspNetCore.Identity;
namespace ICPC_WebSite_Backend.Data.Models
{
    public class User:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? University { get; set; }
        public string? Faculty { get; set; }
        public string? SecondaryEmail  { get; set; }
        public string? FaceBookProfile { get; set; }
        public int? CommunityId{ get; set; }
        public Community Community{ get; set; }
        public ICollection<CommunityRequest> CommunityRequests{ get; set; }
        public ICollection<TrainingRequest> TrainingRequests{ get; set; }
    }
}
