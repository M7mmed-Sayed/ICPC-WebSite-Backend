using ICPC_WebSite_Backend.Data.Models;
using ICPC_WebSite_Backend.Data.ReturnObjects.Models;
using ICPC_WebSite_Backend.Models.DTO;

namespace ICPC_WebSite_Backend.Repository
{
    public interface IAccountRepository
    {
        Task<Response> SignUpAsync(SignUp user);
        Task<Response> LoginAsync(SignIn signInModel);

        Task<Response> SendToken(User AppUser);
        Task<Response> Confirm(string id, string token);
        Task<Response> RemoveRoleAsync(UserRole userRole);
        Task<Response> AddRoleAsync(UserRole userRole);
    }
}