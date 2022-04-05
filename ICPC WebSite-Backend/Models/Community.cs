using System.ComponentModel.DataAnnotations;

namespace ICPC_WebSite_Backend.Models
{
    public class Community
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string About { get; set; }
        [EmailAddress]
        public string OfficialMail { get; set; }
        public string RequesterId { get; set; }
        public bool IsApproved { get; set; } = false;
    }
}
