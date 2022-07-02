using System.ComponentModel.DataAnnotations;

namespace UtilityLibrary.ModelsDTO
{
    public class CommunityDto
    {
        public string Name { get; set; }
        public string About { get; set; }
        [EmailAddress]
        public string OfficialMail { get; set; }
        public string RequesterEmail { get; set; }

    }
}
