namespace ICPC_WebSite_Backend.Data.Models.ReturnObjects
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string University { get; set; }
        public string Faculty { get; set; }
        public string CodeForcesHandle { get; set; }
    }
}
