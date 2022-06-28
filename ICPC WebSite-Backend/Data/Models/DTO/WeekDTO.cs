using System.ComponentModel.DataAnnotations;

namespace ICPC_WebSite_Backend.Data.Models.DTO
{
    public class WeekDto
    {
        [Required] public string Name { get; set; }
        public string Description { get; set; }
        public bool IsTemplate { get; set; }
        public int CommunityId { get; set; }
    }
}