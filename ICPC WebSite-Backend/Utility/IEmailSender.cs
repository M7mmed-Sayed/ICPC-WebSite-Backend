namespace ICPC_WebSite_Backend.Utility
{
    public interface IEmailSender
    {
        void SendEmail(string emailTo, string token, bool isHTML = true);
    }
}