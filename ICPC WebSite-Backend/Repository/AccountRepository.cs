using ICPC_WebSite_Backend.Models;
using ICPC_WebSite_Backend.Utility;
using Microsoft.AspNetCore.Identity;

namespace ICPC_WebSite_Backend.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailSender _emailSender;


        public AccountRepository(UserManager<User> userManager, IEmailSender emailSender) {
            _userManager = userManager;
            _emailSender = emailSender;
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
                Errors = result.Errors,
                Succeeded = result.Succeeded,
                Email = AppUser.Email,
                UserId = AppUser.Id,
                Username = AppUser.UserName
            };
            SendToken(AppUser);
            return ret;
        }
        public async void SendToken(User AppUser) {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(AppUser);
            token = System.Web.HttpUtility.UrlEncode(token);
            var message = $"Hello {AppUser.FirstName}<br>";
            var domain = "";
            message += $"This is your confirmation <a href=\"{domain}/api/Account/confirm?id={AppUser.Id}&token={token}\">Link</a>";
            var subject = "Competitve Programing Confirmaition";
            _emailSender.SendEmail(AppUser.Email, subject, message);
        }
        public async Task<IdentityResult> Confirm(string id, string token) {
            var user = await _userManager.FindByIdAsync(id);

            return await _userManager.ConfirmEmailAsync(user, token);
        }
    }
}
