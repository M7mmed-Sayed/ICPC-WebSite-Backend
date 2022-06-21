using System.Net;
using System.Net.Mail;
using ICPC_WebSite_Backend.Data.Response;

namespace ICPC_WebSite_Backend.Utility;

public class EmailSender : IEmailSender
{
    private readonly string _email;
    private readonly int _mailSubmissionPort;
    private readonly string _password;
    private readonly string _smtpServerAddress;

    public EmailSender(string email, string password, string smtpServerAddress, int mailSubmissionPort)
    {
        _email = email;
        _password = password;
        _smtpServerAddress = smtpServerAddress;
        _mailSubmissionPort = mailSubmissionPort;
    }

    public Response SendEmail(string emailTo, string mailSubject, string mailBody, bool isHtml = true)
    {
        var validate = ValidConfiguration();

        if (!validate.Succeeded)
        {
            Console.WriteLine("Email Sender isn't configured");
            return validate;
        }

        try
        {
            var mailMessage = new MailMessage(_email, emailTo);
            mailMessage.Subject = mailSubject;
            mailMessage.Body = mailBody;
            mailMessage.IsBodyHtml = isHtml;
            var smtpClient = new SmtpClient(_smtpServerAddress, _mailSubmissionPort);
            smtpClient.Credentials = new NetworkCredential
            {
                UserName = _email,
                Password = _password
            };
            smtpClient.EnableSsl = true;

            smtpClient.Send(mailMessage);
            return ResponseFactory.Ok();
        }
        catch (Exception ex)
        {
            var err = new Error {Code = ex.Message, Description = ex.InnerException?.Message ?? ""};
            return ResponseFactory.Fail(err);
        }
    }

    private Response ValidConfiguration()
    {
        var errorsLists = new List<Error>();
        if (string.IsNullOrEmpty(_email))
            errorsLists.Add(ErrorsList.EmailSenderEmailIsNotConfigured);
        else if (!Validate.IsValidEmail(_email))
            errorsLists.Add(ErrorsList.InvalidEmail);

        if (string.IsNullOrEmpty(_password)) errorsLists.Add(ErrorsList.EmailSenderPasswordIsNotConfigured);

        if (string.IsNullOrEmpty(_smtpServerAddress))
            errorsLists.Add(ErrorsList.EmailSenderSmtpServerAddressIsNotConfigured);

        if (_mailSubmissionPort == 0)
            errorsLists.Add(ErrorsList.EmailSenderMailSubmissionPortIsNotConfigured);

        return errorsLists.Any() ? ResponseFactory.Fail(errorsLists) : ResponseFactory.Ok();
    }
}