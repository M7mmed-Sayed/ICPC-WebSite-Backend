using ICPC_WebSite_Backend.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UtilityLibrary.ModelsDTO;

namespace ICPC_WebSite_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestRoleController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;

        public TestRoleController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }
        [Authorize(Roles = "CommunityLeader")]
        [HttpGet("CommunityLeader")]
        public async Task<IActionResult> Index1()
        {
            return Ok("CommunityLeader ");
        }
        [Authorize(Roles = "HeadOfTraining")]
        [HttpGet("HeadOfTraining")]
        public async Task<IActionResult> Index2()
        {
            return Ok("HeadOfTraining ");
        }
        [Authorize(Roles = "TrainingManager")]
        [HttpGet("TrainingManager")]
        public async Task<IActionResult> Index3()
        {
            return Ok("TrainingManager ");
        }
        [Authorize(Roles = "Mentor")]
        [HttpGet("Mentor")]
        public async Task<IActionResult> Index4()
        {
            return Ok("Mentor ");
        }
        [Authorize(Roles = "Trainee")]
        [HttpGet("Trainee")]
        public async Task<IActionResult> Index5()
        {
            return Ok("Trainee ");
        }
        [HttpPost("Create-Role")]
        public async Task<IActionResult> CreateRole(UserRole userRole)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _accountRepository.AddRoleAsync(userRole);

            

            return Ok(result);
        }
        [HttpPost("remove-Role")]
        public async Task<IActionResult> RemoveRole(UserRole userRole)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _accountRepository.RemoveRoleAsync(userRole);



            return Ok(result);
        }

    }
}
