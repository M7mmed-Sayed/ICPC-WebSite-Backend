using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Web;
using ICPC_WebSite_Backend.Configurations;
using ICPC_WebSite_Backend.Data;
using ICPC_WebSite_Backend.Data.Models;
using ICPC_WebSite_Backend.Data.Models.ReturnObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using UtilityLibrary.ModelsDTO;
using UtilityLibrary.Response;
using UtilityLibrary.Utility;

namespace ICPC_WebSite_Backend.Repository;

public class AccountRepository : IAccountRepository
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IConfiguration _configuration;
    private readonly IEmailSender _emailSender;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;

    public AccountRepository(
        ApplicationDbContext applicationDbContext, UserManager<User> userManager, SignInManager<User> signInManager,
        IConfiguration configuration, IEmailSender emailSender, RoleManager<IdentityRole> roleManager)
    {
        _applicationDbContext = applicationDbContext;
        _userManager = userManager;
        _emailSender = emailSender;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }

    public async Task<Response<SignUpResponse>> SignUpAsync(SignUp user)
    {
        try
        {
            var appUser = new User
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email
            };
            var result = await _userManager.CreateAsync(appUser, user.Password);
            if (result.Succeeded == false) return result.ToApplicationResponse<SignUpResponse>();
            var data = new SignUpResponse
            {
                Email = appUser.Email,
                UserId = appUser.Id,
                Username = appUser.UserName
            };
            var sendEmailResult = await SendEmailConfirmationTokenAsync(appUser.Id);

            return !sendEmailResult.Succeeded
                ? ResponseFactory.Fail(sendEmailResult.Errors!, data)
                : ResponseFactory.Ok(data);
        }
        catch (Exception ex)
        {
            return ResponseFactory.FailFromException<SignUpResponse>(ex);
        }
    }
    public async Task<Response> SendEmailConfirmationTokenAsync(string userId) {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) {
            
            return ResponseFactory.Fail(ErrorsList.CannotFindUser);;
        }
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        token = System.Web.HttpUtility.UrlEncode(token);
        var domain = Config.PathBase;
        var confirmationLink = new UriBuilder(domain)
        {
            //confirmationLink.Path = _httpContext.Request.Path;
            Path = "api/Account/Confirm",
            Query = $"id={user.Id}&token={token}"
        };
        var subject = "Competitive Programing Email Confirmation";
        var message = $"Hello {user.FirstName}<br>";
        message += $"This is your confirmation <a href=\"{confirmationLink}\">Link</a>";
        return _emailSender.SendEmail(user.Email, subject, message);
    }


    public async Task<Response> Confirm(string id, string token)
    {
        var user = await _userManager.FindByIdAsync(id);

        var confirmResult = await _userManager.ConfirmEmailAsync(user, token);
        return confirmResult.ToApplicationResponse();
    }

    public async Task<Response<LoginResponse>> LoginAsync(SignIn signInModel)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(signInModel.Email);

            if (user == null) return ResponseFactory.Fail<LoginResponse>(ErrorsList.CannotFindUser);

            var result = await _signInManager.PasswordSignInAsync(user, signInModel.Password, false, false);

            if (!result.Succeeded) return ResponseFactory.Fail<LoginResponse>(ErrorsList.IncorrectEmailOrPassword);

            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var rolesClaims = roles.Select(role => new Claim("roles", role)).ToList();

        var authClaims = new List<Claim>
        {
            new(ClaimTypes.Name, signInModel.Email),
            new(ClaimTypes.NameIdentifier, user.Id),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        }.Union(userClaims).Union(rolesClaims);
        var authSigninKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                _configuration["JWT:ValidIssuer"],
                _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigninKey, SecurityAlgorithms.HmacSha256Signature)
            );
            var data = new LoginResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                UserId = user.Id,
                Email = user.Email,
                Username = user.UserName,
                University = user.University,
                Faculty = user.Faculty
            };
            return ResponseFactory.Ok(data);
        }
        catch (Exception ex)
        {
            return ResponseFactory.FailFromException<LoginResponse>(ex);
        }
    }

    public async Task<Response> AddRoleAsync(UserRole userRole)
    {
        var user = await _userManager.FindByEmailAsync(userRole.UserEmail);

        if (user == null) return ResponseFactory.Fail(ErrorsList.CannotFindUser);

        if (!await _roleManager.RoleExistsAsync(userRole.Role)) return ResponseFactory.Fail(ErrorsList.InvalidRoleName);

        if (await _userManager.IsInRoleAsync(user, userRole.Role))
            return ResponseFactory.Fail(ErrorsList.UserHaveSameRole);

        var result = await _userManager.AddToRoleAsync(user, userRole.Role);
        return result.ToApplicationResponse();
    }

    public async Task<Response> RemoveRoleAsync(UserRole userRole)
    {
        var user = await _userManager.FindByEmailAsync(userRole.UserEmail);

        if (user == null) return ResponseFactory.Fail(ErrorsList.CannotFindUser);

        if (!await _roleManager.RoleExistsAsync(userRole.Role)) return ResponseFactory.Fail(ErrorsList.InvalidRoleName);

        if (!await _userManager.IsInRoleAsync(user, userRole.Role))
            return ResponseFactory.Fail(ErrorsList.UserHasNotThisRole);

        var result = await _userManager.RemoveFromRoleAsync(user, userRole.Role);
        return result.ToApplicationResponse();
    }

    public async Task<Response<UserDto>> GetUserData(string userId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) return ResponseFactory.Fail<UserDto>(ErrorsList.CannotFindUser);
            var data = new UserDto
            {
                UserId = userId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                FaceBookProfile = user.FaceBookProfile,
                Faculty = user.Faculty,
                University = user.University,
                Email = user.Email,
                SecondaryEmail = user.SecondaryEmail
            };
            return ResponseFactory.Ok(data);
        }
        catch (Exception ex)
        {
            return ResponseFactory.FailFromException<UserDto>(ex);
        }
    }

    public async Task<Response> UpdateUserData(string userId, UserDto userDto)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)

                return ResponseFactory.Fail(ErrorsList.CannotFindUser);


            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.UserName = userDto.UserName;
            user.PhoneNumber = userDto.PhoneNumber;
            user.FaceBookProfile = userDto.FaceBookProfile;
            user.Faculty = userDto.Faculty;
            user.University = userDto.University;
            user.Email = userDto.Email;
            user.SecondaryEmail = userDto.SecondaryEmail;
            await _applicationDbContext.SaveChangesAsync();

            return ResponseFactory.Ok();
        }
        catch (Exception ex)
        {
            return ResponseFactory.FailFromException(ex);
        }
    }

    public async Task<Response> ForgetPassword(string userId)
    {
        try
        {
            var appUser = await _userManager.FindByIdAsync(userId);
        if (appUser == null) return ResponseFactory.Fail(ErrorsList.InvalidEmail);
        var token = await _userManager.GeneratePasswordResetTokenAsync(appUser);
        token = HttpUtility.UrlEncode(token);
        var domain = Config.PathBase;
         var resetPasswordLink = new UriBuilder(domain)
         {
             Path = "api/Account/resetpassword",
             Query = $"id={userId}&token={token}"
         };
        var message = $"Hello {appUser.FirstName}<br>";
        message +=
            $"You recently requested to reset your password for your Competitive Programing  account." +
            $"Click the button below to reset it. " +
            $"This password reset is only valid for the next 60 minutes." +
            $"<a href=\"{resetPasswordLink}\">Link</a>";
        const string subject = "Competitive Programing ResetPassword";
        return _emailSender.SendEmail(appUser.Email, subject, message);
        }
        catch (Exception ex)
        {
            return ResponseFactory.FailFromException(ex);
        }
    }

    public async Task<Response> ResetPassword(string id, string token, ResetPassword resetPassword)
    {
        try
        {
            if (string.CompareOrdinal(resetPassword.Password, resetPassword.ConfirmPassword) != 0)
                return ResponseFactory.Fail(ErrorsList.PasswordDonotMatch);
            var appUser = await _userManager.FindByIdAsync(id);
            if (appUser == null) return ResponseFactory.Fail(ErrorsList.CannotFindUser);
            var result = await _userManager.ResetPasswordAsync(appUser, token, resetPassword.Password);
            return result.Succeeded ? ResponseFactory.Ok() : result.ToApplicationResponse();
        }
        catch (Exception ex)
        {
            return ResponseFactory.FailFromException(ex);
        }
    }

    public async Task<Response> ChangePassword(ChangePassword changePassword)
    {
        try
        {
        if (string.CompareOrdinal(changePassword.NewPassword, changePassword.ConfirmPassword) != 0)
            return ResponseFactory.Fail(ErrorsList.PasswordDonotMatch);
        var appUser=await _userManager.FindByIdAsync(changePassword.userId);
        var result = await _userManager.ChangePasswordAsync(appUser, changePassword.CurrentPassword,
            changePassword.CurrentPassword);
        if (result.Succeeded) return ResponseFactory.Ok();
        var errors = result.Errors.Select(err => new Error() { Code = err.Code, Description = err.Description }).ToList();
        return  ResponseFactory.Fail(errors);

        }
        catch (Exception ex)
        {
            return ResponseFactory.FailFromException(ex);
        }
    }

    public async Task<Response> AddAdmin(string userEmail)
    {
        var user = await _userManager.FindByEmailAsync(userEmail);
        if (user == null) return ResponseFactory.Fail(ErrorsList.CannotFindUser);
        if (!await _roleManager.RoleExistsAsync(RolesList.Administrator))
            return ResponseFactory.Fail(ErrorsList.InvalidRoleName);
        if (await _userManager.IsInRoleAsync(user, RolesList.Administrator))
            return ResponseFactory.Fail(ErrorsList.UserHaveSameRole);
        var result = await _userManager.AddToRoleAsync(user, RolesList.Administrator);
        return result.ToApplicationResponse();
    }

    public async Task<Response> RemoveAdmin(string userEmail)
    {
        var user = await _userManager.FindByEmailAsync(userEmail);
        if (user == null) return ResponseFactory.Fail(ErrorsList.CannotFindUser);
        if (!await _userManager.IsInRoleAsync(user, RolesList.Administrator))
            return ResponseFactory.Fail(ErrorsList.UserHasNotThisRole);
        var result = await _userManager.RemoveFromRoleAsync(user, RolesList.Administrator);
        return result.ToApplicationResponse();
    }
}