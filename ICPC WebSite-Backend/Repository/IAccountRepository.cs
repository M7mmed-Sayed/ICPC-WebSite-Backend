using ICPC_WebSite_Backend.Models;
using Microsoft.AspNetCore.Identity;

namespace ICPC_WebSite_Backend.Repository
{
    public interface IAccountRepository
    {
        Task<ValidateResponse> SignUpAsync(SignUp user);
        Task<ValidateResponse> LoginAsync(SignIn signInModel);

        Task<ValidateResponse> SendToken(User AppUser);
        Task<ValidateResponse> Confirm(string id, string token);
        Task<ValidateResponse> RemoveRoleAsync(UserRole userRole);
        Task<ValidateResponse> AddRoleAsync(UserRole userRole);
    }
}