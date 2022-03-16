using ICPC_WebSite_Backend.Models;
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

        public async Task<SignUpResponse> SignUpAsync(SignUp user) {
            var AppUser = new User {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email
                
            };
            var result = await _userManager.CreateAsync(AppUser, user.Password);
            var ret = new SignUpResponse {
                Succeeded = result.Succeeded,
            };
            foreach (var err in result.Errors) {
                ret.Errors.Add(new Error() {
                    Code = err.Code,
                    Description = err.Description,
                });
            }
            if (!result.Succeeded) {
                return ret;
            }
            ret.Email = AppUser.Email;
            ret.UserId = AppUser.Id;
            ret.Username = AppUser.UserName;
            var SendEmailResult = await SendToken(AppUser);
            if (!SendEmailResult.Succeeded) {
                ret.Succeeded = false;
                ret.Errors.AddRange(SendEmailResult.Errors);
            }
          //  await _userManager.AddToRoleAsync(AppUser, "CommunityLeader");
            return ret;
        }
        public async Task<ValidateResponse> SendToken(User AppUser) {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(AppUser);
            token = System.Web.HttpUtility.UrlEncode(token);
            var message = $"Hello {AppUser.FirstName}<br>";
            var domain = "";
            message += $"This is your confirmation <a href=\"{domain}/api/Account/confirm?id={AppUser.Id}&token={token}\">Link</a>";
            var subject = "Competitve Programing Confirmaition";
            return _emailSender.SendEmail(AppUser.Email, subject, message);
        }
        public async Task<IdentityResult> Confirm(string id, string token) {
            var user = await _userManager.FindByIdAsync(id);

            return await _userManager.ConfirmEmailAsync(user, token);
        }
        public async Task<SignInRespones> LoginAsync(SignIn signInModel) {
            var user = await _userManager.FindByEmailAsync(signInModel.Email);
            if (user == null) return null;
            var result = await _signInManager.PasswordSignInAsync(user, signInModel.Password, false, false);
            var userClaims= await _userManager.GetClaimsAsync(user);
            var roles= await _userManager.GetRolesAsync(user);
            var rolesClaims = new List<Claim>();
            foreach (var role in roles) rolesClaims.Add(new Claim("roles", role));
            if (!result.Succeeded)
                return null;

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

            return new SignInRespones {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                UserId = user.Id,
                Email = user.Email,
                Username = user.UserName
            };
        }
        public async Task<String> AddRoleAsync(UserRole userRole)
        {
            var user = await _userManager.FindByIdAsync(userRole.UserId);
            if (user is null||!await _roleManager.RoleExistsAsync(userRole.Role)) return "invaled User or id";
            if(await _userManager.IsInRoleAsync(user, userRole.Role))
                return "User already has  this Role";
            var result = await _userManager.AddToRoleAsync(user, userRole.Role);
            if (result.Succeeded)
                return String.Empty;
            return "Error";

        }
        public async Task<String> RemoveRoleAsync(UserRole userRole)
        {
            var user = await _userManager.FindByIdAsync(userRole.UserId);
            if (user is null || !await _roleManager.RoleExistsAsync(userRole.Role)) return "invaled User or id";
            if (!await _userManager.IsInRoleAsync(user, userRole.Role))
                return "User has'nt  this Role";
            var result = await _userManager.RemoveFromRoleAsync(user, userRole.Role);
            if (result.Succeeded)
                return String.Empty;
            return "Error";

        }
    }
}
