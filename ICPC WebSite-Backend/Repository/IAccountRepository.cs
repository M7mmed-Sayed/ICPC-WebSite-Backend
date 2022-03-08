using ICPC_WebSite_Backend.Models;
using Microsoft.AspNetCore.Identity;

namespace ICPC_WebSite_Backend.Repository
{
    public interface IAccountRepository
    {
        Task<SignUpResponse> SignUpAsync(SignUp user);
        void SendToken(User AppUser);
        Task<IdentityResult> Confirm(string id, string token);

    }
}