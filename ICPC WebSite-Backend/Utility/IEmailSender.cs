using ICPC_WebSite_Backend.Models;

namespace ICPC_WebSite_Backend.Utility
{
    public interface IEmailSender
    {
        ValidateResponse SendEmail(string emailTo, string token, bool isHTML = true);
    }
}