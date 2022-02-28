using ICPC_WebSite_Backend.Models;
using Microsoft.AspNetCore.Identity;

namespace ICPC_WebSite_Backend.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<User> _userManager;


        public AccountRepository(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public async Task<SignUpResponse> SignUpAsync(SignUp user)
        {
            var AppUser = new User
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email
            };
            var result = await _userManager.CreateAsync(AppUser, user.Password);
            var ret = new SignUpResponse
            {
                Errors = result.Errors,
                Succeeded = result.Succeeded,
                Email = AppUser.Email,
                UserId = AppUser.Id,
                Username = AppUser.UserName
            };
            return ret;
        }
    }
}
