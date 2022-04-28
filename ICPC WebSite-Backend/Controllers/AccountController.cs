using ICPC_WebSite_Backend.Models.DTO;
using ICPC_WebSite_Backend.Repository;
using ICPC_WebSite_Backend.Utility;
using Microsoft.AspNetCore.Mvc;

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
        [HttpPost("SendConfirmationMail")]
        public async Task<IActionResult> SendConfirmationMail([FromQuery] string userId) {
            var result = await _accountRepository.SendToken(userId);

            if (!result.Succeeded) {
                return Unauthorized(result);
            }

            return Ok(result);
        }
    }
}
