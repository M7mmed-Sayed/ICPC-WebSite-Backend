using System.ComponentModel.DataAnnotations;

namespace ICPC_WebSite_Backend.Data.Models.DTO;

public class SheetDto
{
    [Required]
    public string Url { get; set; }
    public string Name { get; set; }
    public int CommunityId { get; set; }

}