using ICPC_WebSite_Backend.Data;
using ICPC_WebSite_Backend.Models;
using ICPC_WebSite_Backend.Repository;
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
        public static void RegisterRepos(this IServiceCollection Services, IConfiguration Configuration) {
            Services.AddTransient<IAccountRepository, AccountRepository>();
            Services.AddTransient<ICommunityRepository, CommunityRepository>();
            var myEmail = Configuration["email"];
            var myPassword = Configuration["emailpassword"];
            var SMTPServerAddress = Configuration["SMTPServerAddress"];
            var mailSubmissionPort = Convert.ToInt32(Configuration["mailSubmissionPort"]);
            Services.AddTransient<IEmailSender, EmailSender>(op => new EmailSender(myEmail, myPassword, SMTPServerAddress, mailSubmissionPort));

        }
        public static void RegisterAuth(this IServiceCollection Services, IConfiguration Configuration) {
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
                    ValidAudience = Configuration["JWT:ValidAudience"],
                    ValidIssuer = Configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))
                };
            });
        }
        public static void ConfigureDatabase(this IServiceCollection Services, IConfiguration Configuration) {
            Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Default")));
            Services.AddIdentity<User, IdentityRole>(options => options.User.RequireUniqueEmail = true)
                   .AddEntityFrameworkStores<ApplicationDbContext>()
                   .AddDefaultTokenProviders();

        }
    }
}
