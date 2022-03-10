using ICPC_WebSite_Backend.Models;
using System.Net.Mail;

namespace ICPC_WebSite_Backend.Utility
{
    public class EmailSender : IEmailSender
    {
        private readonly string _email;
        private readonly string _password;
        private readonly string _SMTPServerAddress;
        private readonly int _mailSubmissionPort;

        public EmailSender(string email, string password, string SMTPServerAddress, int mailSubmissionPort) {
            _email = email;
            _password = password;
            _SMTPServerAddress = SMTPServerAddress;
            _mailSubmissionPort = mailSubmissionPort;
        }
        public ValidateResponse SendEmail(string emailTo, string MailSubject, string MailBody, bool isHTML = true) {
            var validate = ValidConfiguration();
            if (!validate.Succeeded) {
                Console.WriteLine("Email Sender isn't configured");
                return validate;
            }
            MailMessage mailMessage = new MailMessage(_email, emailTo);
            mailMessage.Subject = MailSubject;
            mailMessage.Body = MailBody;
            mailMessage.IsBodyHtml = isHTML;
            SmtpClient smtpClient = new SmtpClient(_SMTPServerAddress, _mailSubmissionPort);
            smtpClient.Credentials = new System.Net.NetworkCredential() {
                UserName = _email,
                Password = _password
            };
            smtpClient.EnableSsl = true;
            smtpClient.Send(mailMessage);
            return validate;
        }
        public ValidateResponse ValidConfiguration() {
            var result = new ValidateResponse();
            if (String.IsNullOrEmpty(_email)) {
                result.Succeeded = false;
                result.Errors.Add(ErrorsList.EmailSenderEmailIsNotConfigured);
                return result;
            }
            if (!Validate.IsValidEmail(_email)) {
                result.Succeeded = false;
                result.Errors.Add(ErrorsList.InvalidEmail);
                return result;
            }
            if (String.IsNullOrEmpty(_password)) {
                result.Succeeded = false;
                result.Errors.Add(ErrorsList.EmailSenderPasswordIsNotConfigured);
            }
            if (String.IsNullOrEmpty(_SMTPServerAddress)) {
                result.Succeeded = false;
                result.Errors.Add(ErrorsList.EmailSenderSMTPServerAddressIsNotConfigured);
            }
            if (_mailSubmissionPort == null || _mailSubmissionPort == 0) {
                result.Succeeded = false;
                result.Errors.Add(ErrorsList.EmailSenderMailSubmissionPortIsNotConfigured);
            }
            return result;
        }
    }
}
