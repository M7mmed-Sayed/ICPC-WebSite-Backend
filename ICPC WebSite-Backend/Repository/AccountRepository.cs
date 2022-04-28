using ICPC_WebSite_Backend.Configurations;
using ICPC_WebSite_Backend.Data.Models;
using ICPC_WebSite_Backend.Data.ReturnObjects.Models;
using ICPC_WebSite_Backend.Models.DTO;
using ICPC_WebSite_Backend.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ICPC_WebSite_Backend.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountRepository(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration, IEmailSender emailSender, RoleManager<IdentityRole> roleManager) {
            _userManager = userManager;
            _emailSender = emailSender;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        public async Task<Response> SignUpAsync(SignUp user) {
            var ret = new Response();
            try {
                var AppUser = new User {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = user.UserName,
                    Email = user.Email
                };
                var result = await _userManager.CreateAsync(AppUser, user.Password);
                ret.Succeeded = result.Succeeded;
                foreach (var err in result.Errors) {
                    ret.Errors.Add(new Error() {
                        Code = err.Code,
                        Description = err.Description,
                    });
                }
                if (!result.Succeeded) {
                    return ret;
                }
                ret.Data = new {
                    Email = AppUser.Email,
                    UserId = AppUser.Id,
                    Username = AppUser.UserName
                };
                var SendEmailResult = await SendToken(AppUser.Id);
                if (!SendEmailResult.Succeeded) {
                    ret.Succeeded = false;
                    ret.Errors.AddRange(SendEmailResult.Errors);
                }
            }
            catch (Exception ex) {
                ret.Succeeded = false;
                var err = new Error() { Code = ex.Message };
                if (ex.InnerException != null) err.Description = ex.InnerException.Message;

                ret.Errors.Add(err);
            }
            return ret;
        }
        public async Task<Response> SendToken(string userId) {
            var ret = new Response();
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) {
                ret.Succeeded = false;
                ret.Errors.Add(ErrorsList.CannotFindUser);
                return ret;
            }
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            token = System.Web.HttpUtility.UrlEncode(token);

            var domain = Config.PathBase;
            UriBuilder confirmationLink = new UriBuilder(domain);
            //confirmationLink.Path = _httpContext.Request.Path;
            confirmationLink.Path = "api/Account/Confirm";
            confirmationLink.Query = $"id={user.Id}";
            confirmationLink.Query += $"&token={token}";

            var subject = "Competitve Programing Email Confirmaition";
            var message = $"Hello {user.FirstName}<br>";
            message += $"This is your confirmation <a href=\"{confirmationLink}\">Link</a>";
            ret = _emailSender.SendEmail(user.Email, subject, message);
            return ret;
        }
        public async Task<Response> Confirm(string id, string token) {
            var user = await _userManager.FindByIdAsync(id);

            var confirmResult = await _userManager.ConfirmEmailAsync(user, token);
            var ret = new Response() { Succeeded = confirmResult.Succeeded };
            foreach (var err in confirmResult.Errors) {
                ret.Errors.Add(new Error() { Code = err.Code, Description = err.Description });
            }
            return ret;
        }
        public async Task<Response> LoginAsync(SignIn signInModel) {
            var ret = new Response();
            var user = await _userManager.FindByEmailAsync(signInModel.Email);
            if (user == null) {
                ret.Succeeded = false;
                ret.Errors.Add(ErrorsList.CannotFindUser);
                return ret;
            }
            var result = await _signInManager.PasswordSignInAsync(user, signInModel.Password, false, false);
            ret.Succeeded = result.Succeeded;

            if (!ret.Succeeded) {
                ret.Errors.Add(ErrorsList.IncorrectEmailOrPassword);
                return ret;
            }
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var rolesClaims = new List<Claim>();
            foreach (var role in roles) rolesClaims.Add(new Claim("roles", role));

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, signInModel.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            }.Union(userClaims).Union(rolesClaims);
            var authSigninKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigninKey, SecurityAlgorithms.HmacSha256Signature)
                );
            ret.Data = new SignInRespones {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                UserId = user.Id,
                Email = user.Email,
                Username = user.UserName
            };
            return ret;
        }
        public async Task<Response> AddRoleAsync(UserRole userRole) {
            var ret = new Response();
            var user = await _userManager.FindByEmailAsync(userRole.UserEmail);
            if (user == null) { ret.Succeeded = false; ret.Errors.Add(ErrorsList.CannotFindUser); };
            if (!await _roleManager.RoleExistsAsync(userRole.Role)) { ret.Succeeded = false; ret.Errors.Add(ErrorsList.InvalidRoleName); }
            if (ret.Succeeded) {
                if (await _userManager.IsInRoleAsync(user, userRole.Role)) { ret.Succeeded = false; ret.Errors.Add(ErrorsList.DuplicateRoleName); }
                await _userManager.AddToRoleAsync(user, userRole.Role);
            }
            return ret;
        }
        public async Task<Response> RemoveRoleAsync(UserRole userRole) {
            var ret = new Response();
            var user = await _userManager.FindByEmailAsync(userRole.UserEmail);
            if (user == null) { ret.Succeeded = false; ret.Errors.Add(ErrorsList.CannotFindUser); };
            if (!await _roleManager.RoleExistsAsync(userRole.Role)) { ret.Succeeded = false; ret.Errors.Add(ErrorsList.InvalidRoleName); }
            if (ret.Succeeded) {
                if (!await _userManager.IsInRoleAsync(user, userRole.Role)) { ret.Succeeded = false; ret.Errors.Add(ErrorsList.UserHasNotThisRole); }
                await _userManager.RemoveFromRoleAsync(user, userRole.Role);
            }
            return ret;

        }
    }
}
