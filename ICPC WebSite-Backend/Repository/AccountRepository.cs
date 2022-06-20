﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Web;
using ICPC_WebSite_Backend.Data;
using ICPC_WebSite_Backend.Data.Models;
using ICPC_WebSite_Backend.Data.Models.DTO;
using ICPC_WebSite_Backend.Data.Models.ReturnObjects;
using ICPC_WebSite_Backend.Data.Response;
using ICPC_WebSite_Backend.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

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
            var AppUser = new User
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email
            };
            var result = await _userManager.CreateAsync(AppUser, user.Password);
            if (result.Succeeded == false) return result.ToApplicationResponse<SignUpResponse>();
            var data = new SignUpResponse
            {
                Email = AppUser.Email,
                UserId = AppUser.Id,
                Username = AppUser.UserName
            };
            var sendEmailResult = await SendToken(AppUser);

            return !sendEmailResult.Succeeded
                ? ResponseFactory.Fail(sendEmailResult.Errors!, data)
                : ResponseFactory.Ok(data);
        }
        catch (Exception ex)
        {
            return ResponseFactory.FailFromException<SignUpResponse>(ex);
        }
    }

    public async Task<Response> SendToken(User AppUser)
    {
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(AppUser);
        token = HttpUtility.UrlEncode(token);
        var message = $"Hello {AppUser.FirstName}<br>";
        var domain = "";
        message +=
            $"This is your confirmation <a href=\"{domain}/api/Account/confirm?id={AppUser.Id}&token={token}\">Link</a>";
        var subject = "Competitve Programing Confirmaition";
        return _emailSender.SendEmail(AppUser.Email, subject, message);
    }

    public async Task<Response> Confirm(string id, string token)
    {
        var user = await _userManager.FindByIdAsync(id);

        var confirmResult = await _userManager.ConfirmEmailAsync(user, token);
        return confirmResult.ToApplicationResponse();
    }

    public async Task<Response<LoginResponse>> LoginAsync(SignIn signInModel)
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

    public async Task<Response> AddRoleAsync(UserRole userRole)
    {
        var user = await _userManager.FindByEmailAsync(userRole.UserEmail);

        if (user == null) return ResponseFactory.Fail(ErrorsList.CannotFindUser);

        if (!await _roleManager.RoleExistsAsync(userRole.Role)) return ResponseFactory.Fail(ErrorsList.InvalidRoleName);

        if (await _userManager.IsInRoleAsync(user, userRole.Role))
            return ResponseFactory.Fail(ErrorsList.DuplicateRoleName);

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

    public async Task<Response<UserDTO>> GetUserData(string userId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) return ResponseFactory.Fail<UserDTO>(ErrorsList.CannotFindUser);
            var data = new UserDTO
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
            return ResponseFactory.FailFromException<UserDTO>(ex);
        }
    }

    public async Task<Response> UpdateUserData(string userId, UserDTO userDto)
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
}