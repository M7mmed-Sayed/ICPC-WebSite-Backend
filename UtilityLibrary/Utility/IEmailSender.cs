namespace UtilityLibrary.Utility;

public interface IEmailSender
{
    Response.Response SendEmail(string emailTo, string mailSubject, string mailBody, bool isHtml = true);
}