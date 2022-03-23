namespace ICPC_WebSite_Backend.Models
{
    public class Response
    {
        public bool Succeeded { get; set; } = true;
        public List<Error> Errors { get; set; } = new List<Error>();
        public object Data { get; set; } 
    }
}
