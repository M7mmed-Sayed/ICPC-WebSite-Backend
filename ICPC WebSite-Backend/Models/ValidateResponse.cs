namespace ICPC_WebSite_Backend.Models
{
    public class ValidateResponse
    {
        public bool Succeeded { get; set; } = true;
        public List<Error> Errors { get; set; } = new List<Error>();
    }
}
