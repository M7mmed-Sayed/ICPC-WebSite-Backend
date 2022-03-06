using ICPC_WebSite_Backend.Models;

namespace ICPC_WebSite_Backend.Utility
{
    public interface IEmailSender
    {
        ValidateResponse SendEmail(string emailTo, string MailSubject, string MailBody, bool isHTML = true);
    }
}