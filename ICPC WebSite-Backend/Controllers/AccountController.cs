using ICPC_WebSite_Backend.Models;
using ICPC_WebSite_Backend.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ICPC_WebSite_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;

        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }
        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] SignUp signUpModel)
        {
            var result = await _accountRepository.SignUpAsync(signUpModel);
            if (!result.Succeeded)
            {
                return Unauthorized(result.Errors);
            }
            return Ok(result);
        }
    }
}
