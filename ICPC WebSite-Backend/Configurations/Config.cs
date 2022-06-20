namespace ICPC_WebSite_Backend.Configurations
{
    public static class Config
    {
        public static string myEmail => Get("email", "");
        public static string myPassword => Get("emailpassword", "");
        public static string SMTPServerAddress => Get("SMTPServerAddress", "smtp.gmail.com");
        public static int mailSubmissionPort => Convert.ToInt32(Get("mailSubmissionPort", "587"));
        public static string JWTValidAudience => Get("JWT:ValidAudience");
        public static string JWTValidIssuer => Get("JWT:ValidIssuer");
        public static string JWTSecret => Get("JWT:Secret");
        public static string DefaultConnectionString => Get("ConnectionStrings:Default", "");


        public static string CodeforcesBaseUrl => Get("CodeforcesBaseUrl", "");
        public static string CodeforcesAPIKey => Get("CodeforcesAPIKey", "");
        public static string CodeforcesAPISecret => Get("CodeforcesAPISecret", "");

        private static string Get(string name) {
            return ConfigProvider.Configuration[name] ?? throw new Exception("Configuration for " + name + " not found");
        }
        private static string Get(string name, string defaultValue) {
            return ConfigProvider.Configuration[name] ?? defaultValue;
        }
    }
}
