using System.Net.Mail;

namespace ICPC_WebSite_Backend.Utility
{
    public class EmailSender : IEmailSender
    {
        private readonly string _email;
        private readonly string _password;

        public EmailSender(string email, string password) {
            _email = email;
            _password = password;
        }
        public void SendEmail(string emailTo, string token, bool isHTML = true) {
            MailMessage mailMessage = new MailMessage(_email, emailTo);
            mailMessage.Subject = "ICPC Communities | Confirmation Mail";
            mailMessage.Body = token;
            mailMessage.IsBodyHtml = isHTML;
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.Credentials = new System.Net.NetworkCredential() {
                UserName = _email,
                Password = _password
            };
            smtpClient.EnableSsl = true;
            smtpClient.Send(mailMessage);

        }
    }
}
