using ICPC_WebSite_Backend.Models;

namespace ICPC_WebSite_Backend.Repository
{
    public interface IAccountRepository
    {
        Task<SignUpResponse> SignUpAsync(SignUp user);
        void SendToken(User AppUser);
    }
}