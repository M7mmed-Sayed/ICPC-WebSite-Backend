using System.ComponentModel.DataAnnotations;

namespace UtilityLibrary.ModelsDTO;

public class SheetDto
{
    [Required]
    public string Url { get; set; }
    public string Name { get; set; }
    public string ContestId { get; set; }

    public int CommunityId { get; set; }

}