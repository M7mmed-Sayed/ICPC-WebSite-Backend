using ICPC_WebSite_Backend.Data.Response;

namespace ICPC_WebSite_Backend.Utility;

public interface IEmailSender
{
    Response SendEmail(string emailTo, string mailSubject, string mailBody, bool isHtml = true);
}