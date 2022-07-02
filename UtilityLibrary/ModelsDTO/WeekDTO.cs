using System.ComponentModel.DataAnnotations;

namespace UtilityLibrary.ModelsDTO
{
    public class WeekDto
    {
        [Required] public string Name { get; set; }
        public string Description { get; set; }
        public int CommunityId { get; set; }
    }
}