﻿using ICPC_WebSite_Backend.Models;
using System.Net.Mail;

namespace ICPC_WebSite_Backend.Utility
{
    public class EmailSender : IEmailSender
    {
        private readonly string _email;
        private readonly string _password;
        private readonly int _mailSubmissionPort;

        public EmailSender(string email, string password, int mailSubmissionPort) {
            _email = email;
            _password = password;
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
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", _mailSubmissionPort);
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
            if (_mailSubmissionPort == null || _mailSubmissionPort == 0) {
                result.Succeeded = false;
                result.Errors.Add(ErrorsList.EmailSenderMailSubmissionPortIsNotConfigured);
            }
            return result;
        }
    }
}
