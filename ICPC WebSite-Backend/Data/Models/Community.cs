using System.ComponentModel.DataAnnotations;

namespace ICPC_WebSite_Backend.Data.Models
{
    public class Community
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string About { get; set; }
        [EmailAddress]
        public string OfficialMail { get; set; }
        public string RequesterEmail { get; set; }
        public bool IsApproved { get; set; } = false;
        public ICollection<User> Members { get; set; }
        public ICollection<CommunityRequest> CommunityRequests { get; set; }
        public ICollection<Training> Trainings { get; set; }
        public ICollection<Week> Weeks { get; set; }
        public ICollection<Sheet> Sheets { get; set; }
    }
}