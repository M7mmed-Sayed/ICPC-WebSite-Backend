namespace ICPC_WebSite_Backend.Models
{
    public class UserRoleResponse
    {
        public bool Succeeded { get; set; } = true;
        public List<Error> Errors { get; set; } = new List<Error>();
    }
}
