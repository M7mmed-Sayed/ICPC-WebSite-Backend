namespace ICPC_WebSite_Backend.Data.Models.DTO;

public class CommunityMemberDTO
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public IEnumerable<string> Roles { get; set; }
}