using System.Security.Claims;
using ICPC_WebSite_Backend.Repository;
using ICPC_WebSite_Backend.Security;
using ICPC_WebSite_Backend.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UtilityLibrary.ModelsDTO;
using UtilityLibrary.Utility;

namespace ICPC_WebSite_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IAuthorizationService _authorizationService;

        public AccountController(IAccountRepository accountRepository, IAuthorizationService authorizationService)
        {
            _accountRepository = accountRepository;
            _authorizationService = authorizationService;
        }
        [NonAction]
        private async Task<bool> IsAuthorized(string userId)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, new ClaimResource(ClaimTypes.NameIdentifier, userId), "EditAccess");
            return authorizationResult.Succeeded;
        }
        /// <summary>
        ///  sign up endpoint
        /// </summary>
        /// <param name="signUpModel"></param>
        /// <returns>returns the user id ,email and the username for the registered user</returns>
        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUp signUpModel) {
            var validate = Validate.IsValidSignUp(signUpModel);
            if (!validate.Succeeded)
                return BadRequest(validate);

            var result = await _accountRepository.SignUpAsync(signUpModel);
            if (!result.Succeeded) {
                return Unauthorized(result);
            }
            return Ok(result);
        }
        /// <summary>
        /// confirm the email after sending a token to the registered mail
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns>return true if the email confirmed successfully otherwise return false</returns>
        [HttpGet("confirm")]
        public async Task<IActionResult> Confirm([FromQuery] string id, [FromQuery] string token) {
            if (!await IsAuthorized(id)) return Forbid();

            var result = await _accountRepository.Confirm(id, token);
            if (!result.Succeeded) {
                return BadRequest(result);
            }
            return Ok(result);
        }
        /// <summary>
        /// log the user in
        /// </summary>
        /// <param name="signInModel"></param>
        /// <returns>returns   Token ,UserId , Email ,Username , University and Faculty  for the logged in user</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] SignIn signInModel) {
            var result = await _accountRepository.LoginAsync(signInModel);

            if (!result.Succeeded) {
                return BadRequest(result);
            }

            return Ok(result);
        }
        /// <summary>
        /// get the user data by id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>returns the user data (UserId,FirstName ,LastName ,UserName ,PhoneNumber,FaceBookProfile ,Faculty,University , Email and SecondaryEmail )</returns>
        [HttpGet("")]
        public async Task<IActionResult> GetUserData([FromQuery] string userId) {
            var result = await _accountRepository.GetUserData(userId);

            if (!result.Succeeded) {
                return BadRequest(result);
            }

            return Ok(result);
        }
        /// <summary>
        /// modify user's data
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userDto"></param>
        /// <returns>returns true if the data updated successfully otherwise return false</returns>
        [Authorize]
        [HttpPut("")]
        public async Task<IActionResult> UpdateUserData([FromQuery] string userId, [FromBody] UserDto userDto) {
            if (!await IsAuthorized(userId)) return Forbid();

            var result = await _accountRepository.UpdateUserData(userId, userDto);
            if (!result.Succeeded) {
                return BadRequest(result);
            }
            return Ok(result);
        }

        /// <summary>
        /// request to reset the password by sending an email with a token to the user's mail.
        /// </summary>
        /// <param name="email"></param>
        /// <returns>return true if the mail sent successfully otherwise return false</returns>
        [HttpGet("forgetpassword")]
        public async Task<IActionResult> ForgetPassword([FromQuery] string email) {

            var result = await _accountRepository.ForgetPassword(email);
            if (!result.Succeeded) 
                return BadRequest(result);
            return Ok(result);
        }
        /// <summary>
        /// reset the password with a token that has been sent to the user's mail(after hits forget password)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <param name="resetPassword"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("resetpassword")]
        public async Task<IActionResult> ResetPassword([FromQuery] string id, [FromQuery] string token,[FromBody]ResetPassword resetPassword) {
            if (!await IsAuthorized(id)) return Forbid();

            var result = await _accountRepository.ResetPassword(id,token,resetPassword);
            if (!result.Succeeded) 
                return BadRequest(result);
            return Ok(result);
        }
        /// <summary>
        /// change the password
        /// </summary>
        /// <param name="changePassword"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("changepassword")]
        public async Task<IActionResult> ChangePassword([FromBody]ChangePassword changePassword) {
            if (!await IsAuthorized(changePassword.userId)) return Forbid();
            var result = await _accountRepository.ChangePassword(changePassword);
            if (!result.Succeeded) 
                return BadRequest(result);
            return Ok(result);
        }
        /// <summary>
        /// add new admin to the system
        /// </summary>
        /// <param name="userEmail"></param>
        /// <returns>return true if the user added successfully otherwise return false</returns>
        [HttpPost("addadmin")]
        public async Task<IActionResult> AddAdmin([FromBody]string userEmail) {
            var result = await _accountRepository.AddAdmin(userEmail);
            if (!result.Succeeded) 
                return BadRequest(result);
            return Ok(result);
        }
        /// <summary>
        /// add new admin to the system
        /// </summary>
        /// <param name="userEmail"></param>
        /// <returns>return true if the user removed successfully otherwise return false</returns>
        [HttpDelete("remove-admin")]
        public async Task<IActionResult> RemoveAdmin([FromBody] string userEmail) {
            var result = await _accountRepository.RemoveAdmin(userEmail);
            if (!result.Succeeded) 
                return BadRequest(result);
            return Ok(result);
        }
        
        /// <summary>
        /// request to send a confirmation email
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>return true if the mail sent successfully otherwise return false</returns>
        [Authorize]
        [HttpPost("SendConfirmationMail")]
        public async Task<IActionResult> SendConfirmationMail([FromQuery] string userId) {
            if (!await IsAuthorized(userId)) return Forbid();

            var result = await _accountRepository.SendEmailConfirmationTokenAsync(userId);
            if (!result.Succeeded) {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
