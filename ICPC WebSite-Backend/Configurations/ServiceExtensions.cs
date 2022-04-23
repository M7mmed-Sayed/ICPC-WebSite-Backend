using ICPC_WebSite_Backend.Data;
using ICPC_WebSite_Backend.Repository;
using ICPC_WebSite_Backend.Utility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ICPC_WebSite_Backend.Data.Models;
using Microsoft.OpenApi.Models;

namespace ICPC_WebSite_Backend.Configurations
{
    public static class ServiceExtensions
    {
        public static void AddSwaggerGen(this IServiceCollection Services) {
            Services.AddSwaggerGen(option => {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement{{
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    new string[]{}
                 }});
            });
        }
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
        public static async Task CreateRoles(this IServiceProvider Services) {
            //initializing custom roles 
            var RoleManager = Services.GetRequiredService<RoleManager<IdentityRole>>();
            var roleNames = RolesList.Roles;
            foreach (var roleName in roleNames) {
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist) {
                    IdentityResult roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }
    }
}
