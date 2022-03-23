using ICPC_WebSite_Backend.Utility;
namespace ICPC_WebSite_Backend.Data.ReturnObjects.Models
{
    public class Response
    {
        public bool Succeeded { get; set; } = true;
        public List<Error> Errors { get; set; } = new List<Error>();
        public object Data { get; set; } 
    }
}
