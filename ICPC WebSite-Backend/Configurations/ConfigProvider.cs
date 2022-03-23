using Microsoft.AspNetCore.Hosting;

namespace ICPC_WebSite_Backend.Configurations
{
    public static class ConfigProvider //named with config not configuration ,to avoid conflict with `Microsoft.Extensions.Configuration.ConfigurationProvider`
    {
        public static IConfiguration Configuration;
    }
}
