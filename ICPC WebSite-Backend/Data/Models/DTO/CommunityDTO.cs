using System.ComponentModel.DataAnnotations;

namespace ICPC_WebSite_Backend.Data.Models.DTO
{
    public class CommunityDto
    {
        public string Name { get; set; }
        public string About { get; set; }
        [EmailAddress]
        public string OfficialMail { get; set; }
        public string RequesterId { get; set; }

    }
}
