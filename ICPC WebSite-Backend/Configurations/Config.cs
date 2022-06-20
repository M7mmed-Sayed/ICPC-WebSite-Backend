namespace ICPC_WebSite_Backend.Configurations
{
    public static class Config
    {
        public static string MyEmail => Get("email", "");
        public static string MyPassword => Get("emailpassword", "");
        public static string SmtpServerAddress => Get("SMTPServerAddress", "smtp.gmail.com");
        public static int MailSubmissionPort => Convert.ToInt32(Get("mailSubmissionPort", "587"));
        public static string JwtValidAudience => Get("JWT:ValidAudience");
        public static string JwtValidIssuer => Get("JWT:ValidIssuer");
        public static string JwtSecret => Get("JWT:Secret");
        public static string DefaultConnectionString => Get("ConnectionStrings:Default", "");


        public static string CodeforcesBaseUrl => Get("CodeforcesBaseUrl", "");
        public static string CodeforcesApiKey => Get("CodeforcesAPIKey", "");
        public static string CodeforcesApiSecret => Get("CodeforcesAPISecret", "");

        private static string Get(string name) {
            return ConfigProvider.Configuration[name] ?? throw new Exception("Configuration for " + name + " not found");
        }
        private static string Get(string name, string defaultValue) {
            return ConfigProvider.Configuration[name] ?? defaultValue;
        }
    }
}
