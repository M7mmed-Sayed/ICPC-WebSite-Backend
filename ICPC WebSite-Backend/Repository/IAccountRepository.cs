using ICPC_WebSite_Backend.Data.Models;
using ICPC_WebSite_Backend.Data.Models.ReturnObjects;
using UtilityLibrary.ModelsDTO;
using UtilityLibrary.Response;

namespace ICPC_WebSite_Backend.Repository;

public interface IAccountRepository
{
    Task<Response<SignUpResponse>> SignUpAsync(SignUp user);
    Task<Response<LoginResponse>> LoginAsync(SignIn signInModel);

    Task<Response> SendToken(User appUser);
    Task<Response> Confirm(string id, string token);
    Task<Response> RemoveRoleAsync(UserRole userRole);
    Task<Response> AddRoleAsync(UserRole userRole);
    Task<Response<UserDto>> GetUserData(string userId);
    Task<Response> UpdateUserData(string userId, UserDto userDto);
    Task<Response> ForgetPassword(string email);
    Task<Response> ResetPassword(string id, string token, ResetPassword resetPassword);
    Task<Response> ChangePassword(ChangePassword changePassword);
}