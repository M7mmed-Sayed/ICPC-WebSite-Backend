using ICPC_WebSite_Backend.Models;
using ICPC_WebSite_Backend.Repository;
using ICPC_WebSite_Backend.Utility;
using Microsoft.AspNetCore.Http;
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
                return BadRequest(validate.Errors);

            var result = await _accountRepository.SignUpAsync(signUpModel);
            if (!result.Succeeded) {
                return Unauthorized(result.Errors);
            }
            return Ok(result);
        }
        [HttpGet("confirm")]
        public async Task<IActionResult> Confirm([FromQuery] string id, [FromQuery] string token) {
            var res = await _accountRepository.Confirm(id, token);
            if (res.Succeeded) {
                return Ok(res);
                //return Redirect("\\");
            }
            return BadRequest(res.Errors);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] SignIn signInModel) {
            var result = await _accountRepository.LoginAsync(signInModel);

            if (result == null) {
                return Unauthorized();
            }

            return Ok(result);
        }
    }
}
