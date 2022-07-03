using ICPC_WebSite_Backend.Data.Models;
using ICPC_WebSite_Backend.Data.Models.ReturnObjects;
using UtilityLibrary.ModelsDTO;
using UtilityLibrary.Response;

namespace ICPC_WebSite_Backend.Repository;

public interface IAccountRepository
{
    /// <summary>
    /// sign up a new account
    /// </summary>
    /// <param name="user">this user will have the data for the new account</param>
    /// <returns>if succeeded sign up then return sign up response otherwise return the error</returns>
    Task<Response<SignUpResponse>> SignUpAsync(SignUp user);
    /// <summary>
    /// take an email and password from the user and make him logged in
    /// </summary>
    /// <param name="signInModel">data that being taked from the user to check if he is an acutal user</param>
    /// <returns>return the data of the user if he logged in successfully otherwise it returns an error</returns>
    Task<Response<LoginResponse>> LoginAsync(SignIn signInModel);
    /// <summary>
    /// send an email confirmation to an account
    /// </summary>
    /// <param name="userId">the id of the account</param>
    /// <returns>returns failed or succeeded</returns>
    Task<Response> SendEmailConfirmationTokenAsync(string userId);
    /// <summary>
    /// confirm email 
    /// </summary>
    /// <param name="id">the id of the user</param>
    /// <param name="token">a token to ensure that it is the correct user</param>
    /// <returns>returns failed or succeeded</returns>
    Task<Response> Confirm(string id, string token);
    /// <summary>
    /// get the data of a user
    /// </summary>
    /// <param name="userId">the id of the user</param>
    /// <returns>returns the data of the user in UserDto object</returns>
    Task<Response<UserDto>> GetUserData(string userId);
    /// <summary>
    /// update a user with new data
    /// </summary>
    /// <param name="userId">the id of the user</param>
    /// <param name="userDto">the new data of this user</param>
    /// <returns>returns failed or succeeded</returns>
    Task<Response> UpdateUserData(string userId, UserDto userDto);
    /// <summary>
    /// send a password reset massage for a user's email
    /// </summary>
    /// <param name="email">the email of the user</param>
    /// <returns>returns failed or succeeded</returns>
    Task<Response> ForgetPassword(string userId);
    /// <summary>
    /// reset the password for the user
    /// </summary>
    /// <param name="id">the id of the user</param>
    /// <param name="token">a token for security checks</param>
    /// <param name="resetPassword">reset password signature</param>
    /// <returns>returns failed or succeeded</returns>
    Task<Response> ResetPassword(string id, string token, ResetPassword resetPassword);
    /// <summary>
    /// change a password for a user
    /// </summary>
    /// <param name="changePassword">contains user id, current password and new password </param>
    /// <returns>returns failed or succeeded</returns>
    Task<Response> ChangePassword(ChangePassword changePassword);
    /// <summary>
    /// make user to be an admin
    /// </summary>
    /// <param name="userEmail">email of the user</param>
    /// <returns>returns failed or succeeded</returns>
    Task<Response> AddAdmin(string userEmail);
    /// <summary>
    /// remove adminstration from a user
    /// </summary>
    /// <param name="userEmail">the email of the user</param>
    /// <returns>returns failed or succeeded</returns>
    Task<Response> RemoveAdmin(string userEmail);
}