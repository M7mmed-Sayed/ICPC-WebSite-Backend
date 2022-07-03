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
        [HttpGet("confirm")]
        public async Task<IActionResult> Confirm([FromQuery] string id, [FromQuery] string token) {
            if (!await IsAuthorized(id)) return Forbid();

            var result = await _accountRepository.Confirm(id, token);
            if (!result.Succeeded) {
                return BadRequest(result);
            }
            return Ok(result);
            //return Redirect("\\");
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] SignIn signInModel) {
            var result = await _accountRepository.LoginAsync(signInModel);

            if (!result.Succeeded) {
                return Unauthorized(result);
            }

            return Ok(result);
        }
        [HttpGet("")]
        public async Task<IActionResult> GetUserData([FromQuery] string userId) {
            var result = await _accountRepository.GetUserData(userId);

            if (!result.Succeeded) {
                return Unauthorized(result);
            }

            return Ok(result);
        }
        [Authorize]
        [HttpPut("")]
        public async Task<IActionResult> UpdateUserData([FromQuery] string userId, [FromBody] UserDto userDto) {
            if (!await IsAuthorized(userId)) return Forbid();

            var result = await _accountRepository.UpdateUserData(userId, userDto);
            if (!result.Succeeded) {
                return Unauthorized(result);
            }
            return Ok(result);
        }
        [Authorize]
        [HttpGet("forgetpassword")]
        public async Task<IActionResult> ForgetPassword([FromQuery] string userId) {
            if (!await IsAuthorized(userId)) return Forbid();

            var result = await _accountRepository.ForgetPassword(userId);
            if (!result.Succeeded) 
                return Unauthorized(result);
            return Ok(result);
        }
        [Authorize]
        [HttpPut("resetpassword")]
        public async Task<IActionResult> ResetPassword([FromQuery] string id, [FromQuery] string token,[FromBody]ResetPassword resetPassword) {
            if (!await IsAuthorized(id)) return Forbid();

            var result = await _accountRepository.ResetPassword(id,token,resetPassword);
            if (!result.Succeeded) 
                return Unauthorized(result);
            return Ok(result);
            return Ok();
        }
        [Authorize]
        [HttpPut("changepassword")]
        public async Task<IActionResult> ChangePassword([FromBody]ChangePassword changePassword) {
            if (!await IsAuthorized(changePassword.userId)) return Forbid();
            var result = await _accountRepository.ChangePassword(changePassword);
            if (!result.Succeeded) 
                return Unauthorized(result);
            return Ok(result);
        }
        [HttpPost("addadmin")]
        public async Task<IActionResult> AddAdmin([FromBody]string userEmail) {
            var result = await _accountRepository.AddAdmin(userEmail);
            if (!result.Succeeded) 
                return Unauthorized(result);
            return Ok(result);
        }
        [HttpDelete("remove-admin")]
        public async Task<IActionResult> RemoveAdmin([FromBody] string userEmail) {
            var result = await _accountRepository.RemoveAdmin(userEmail);
            if (!result.Succeeded) 
                return Unauthorized(result);
            return Ok(result);
        }
        
        
        [Authorize]
        [HttpPost("SendConfirmationMail")]
        public async Task<IActionResult> SendConfirmationMail([FromQuery] string userId) {
            if (!await IsAuthorized(userId)) return Forbid();

            var result = await _accountRepository.SendEmailConfirmationTokenAsync(userId);
            if (!result.Succeeded) {
                return Unauthorized(result);
            }
            return Ok(result);
        }
    }
}
