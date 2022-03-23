using ICPC_WebSite_Backend.Data;
using ICPC_WebSite_Backend.Models;
using ICPC_WebSite_Backend.Repository;
using ICPC_WebSite_Backend.Configurations;
using ICPC_WebSite_Backend.Utility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ICPC_WebSite_Backend.Configurations
{
    public static class ServiceExtensions
    {
        // public static void RegisterRepos(this IServiceCollection Services) {
        public static void RegisterRepos(this IServiceCollection Services) {
            Services.AddTransient<IAccountRepository, AccountRepository>();
            Services.AddTransient<ICommunityRepository, CommunityRepository>();
            Services.AddTransient<IEmailSender, EmailSender>(op => new EmailSender(Config.myEmail, Config.myPassword, Config.SMTPServerAddress, Config.mailSubmissionPort));

        }
        public static void RegisterAuth(this IServiceCollection Services) {
            Services.AddAuthentication(option => {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(option => {
                option.SaveToken = true;
                option.RequireHttpsMetadata = false;
                option.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters() {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = Config.JWTValidAudience,
                    ValidIssuer = Config.JWTValidIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config.JWTSecret))
                };
            });
        }
        public static void ConfigureDatabase(this IServiceCollection Services) {
            Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Config.DefaultConnectionString));
            Services.AddIdentity<User, IdentityRole>(options => options.User.RequireUniqueEmail = true)
                   .AddEntityFrameworkStores<ApplicationDbContext>()
                   .AddDefaultTokenProviders();

        }
    }
}
