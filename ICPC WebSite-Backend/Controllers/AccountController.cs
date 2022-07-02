using ICPC_WebSite_Backend.Repository;
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

        public AccountController(IAccountRepository accountRepository) {
            _accountRepository = accountRepository;
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
        [HttpPut("")]
        public async Task<IActionResult> UpdateUserData([FromQuery] string userId, [FromBody] UserDto userDto) {
            var result = await _accountRepository.UpdateUserData(userId, userDto);
            if (!result.Succeeded) {
                return Unauthorized(result);
            }
            return Ok(result);
        }
        [HttpGet("forgetpassword")]
        public async Task<IActionResult> ForgetPassword([FromBody] string userEmail) {
            var result = await _accountRepository.ForgetPassword(userEmail);
            if (!result.Succeeded) 
                return Unauthorized(result);
            return Ok(result);
        }
        [HttpPut("resetpassword")]
        public async Task<IActionResult> ResetPassword([FromQuery] string id, [FromQuery] string token,[FromBody]ResetPassword resetPassword) {
            var result = await _accountRepository.ResetPassword(id,token,resetPassword);
            if (!result.Succeeded) 
                return Unauthorized(result);
            return Ok(result);
            return Ok();
        }
        [HttpPut("changepassword")]
        public async Task<IActionResult> ChangePassword([FromBody]ChangePassword changePassword) {
            var result = await _accountRepository.ChangePassword(changePassword);
            if (!result.Succeeded) 
                return Unauthorized(result);
            return Ok(result);
            return Ok();
        }
        
    }
}
