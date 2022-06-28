using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ICPC_WebSite_Backend.Data.Models
{
    public class Week
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsTemplate { get; set; } = false;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<Material> Materials { get; set; }
        public ICollection<WeekTraining> WeekTraining { get; set; }
        public ICollection<WeekSheet> WeekSheets { get; set; }
        public int CommunityId { get; set; }
        public Community Community { get; set; }
    }
}