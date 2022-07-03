using System.Reflection;
using ICPC_WebSite_Backend.Data;
using ICPC_WebSite_Backend.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ICPC_WebSite_Backend.Data.Models;
using CodeforcesLibrary;
using ICPC_WebSite_Backend.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using UtilityLibrary.Utility;

namespace ICPC_WebSite_Backend.Configurations
{
    public static class ServiceExtensions
    {
        public static void AddSwaggerGen(this IServiceCollection services) {
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo {Title = "ICPC Website API", Version = "v1"});
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
                var fileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var filePath = Path.Combine(AppContext.BaseDirectory, fileName);
                option.IncludeXmlComments(filePath);
            });
        }
        public static void RegisterRepos(this IServiceCollection services) {
            services.AddTransient<IAccountRepository, AccountRepository>();
            services.AddTransient<ICommunityRepository, CommunityRepository>();
            services.AddTransient<IEmailSender, EmailSender>(op => new EmailSender(Config.MyEmail, Config.MyPassword, Config.SmtpServerAddress, Config.MailSubmissionPort));
            services.AddTransient<ICodeforcesRepository, CodeforcesRepository>();
            services.AddTransient<ITrainingRepository, TrainingRepository>();
            services.AddTransient<IWeekRepository, WeekRepository>();
            services.AddTransient<ISheetRepository, SheetRepository>();
            services.AddTransient<IMaterialRepository, MaterialRepository>();
            services.AddTransient(op => new CodeforcesHelper(Config.CodeforcesBaseUrl, Config.CodeforcesApiKey, Config.CodeforcesApiSecret));
        }
        public static void RegisterAuth(this IServiceCollection services) {
            services.AddAuthentication(option => {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(option => {
                option.SaveToken = true;
                option.RequireHttpsMetadata = false;
                option.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters() {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.FromSeconds(5),
                    RequireExpirationTime=true,
                    RequireSignedTokens = true,
                    ValidAudience = Config.JwtValidAudience,
                    ValidIssuer = Config.JwtValidIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config.JwtSecret))
                };
            });
        }
        public static void RegisterAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options => options.AddPolicy("EditAccess", policy =>
            {
                policy.Requirements.Add(new AuthorizationRequirement());
            }));
            services.AddSingleton<IAuthorizationHandler, AuthorizationHandler>();
        }
        public static void ConfigureDatabase(this IServiceCollection services) {
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Config.DefaultConnectionString));
            services.AddIdentity<User, IdentityRole>(options => options.User.RequireUniqueEmail = true)
                   .AddEntityFrameworkStores<ApplicationDbContext>()
                   .AddDefaultTokenProviders();

        }
        public static async Task CreateRoles(this IServiceProvider services) {
            //initializing custom roles 
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var roleNames = RolesList.Roles;
            foreach (var roleName in roleNames) {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist) {
                    IdentityResult roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }
    }
}
