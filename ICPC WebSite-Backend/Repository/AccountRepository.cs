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

        public AccountRepository(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration, IEmailSender emailSender) {
            _userManager = userManager;
            _emailSender = emailSender;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        public async Task<SignUpResponse> SignUpAsync(SignUp user) {
            var ret = new SignUpResponse();
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
                ret.Email = AppUser.Email;
                ret.UserId = AppUser.Id;
                ret.Username = AppUser.UserName;
                var SendEmailResult = await SendToken(AppUser);
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

            if (!result.Succeeded)
                return null;

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, signInModel.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
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
    }
}
