using ICPC_WebSite_Backend.Utility;
namespace ICPC_WebSite_Backend.Data.ReturnObjects.Models
{
    public class SignUpResponse
    {
        public bool Succeeded { get; set; } = true;
        public List<Error> Errors { get; set; }= new List<Error>();
        public string Email { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
    }
}
