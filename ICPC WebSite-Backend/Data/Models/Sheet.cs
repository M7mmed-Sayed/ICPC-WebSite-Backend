namespace ICPC_WebSite_Backend.Data.Models;

public class Sheet
{
    public int Id { get; set; }
    public string Url { get; set; }
    public string Name { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<WeekSheet> WeekSheets { get; set; }
    public int CommunityId { get; set; }
    public Community Community { get; set; }
}