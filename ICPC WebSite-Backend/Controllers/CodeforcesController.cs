using ICPC_WebSite_Backend.Repository;
using Microsoft.AspNetCore.Mvc;

namespace ICPC_WebSite_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CodeforcesController : ControllerBase
    {
        private readonly ICodeforcesRepository _codeforcesRepository;

        public CodeforcesController(ICodeforcesRepository codeforcesRepository)
        {
            _codeforcesRepository = codeforcesRepository;
        }
        [HttpGet("contest/submissions/{contestId}")]
        public async Task<IActionResult> GetSubmissions([FromRoute]string contestId,[FromQuery]string? userCodeforcesHandle)
        {
            var result = await _codeforcesRepository.GetContestSubmissions(contestId, userCodeforcesHandle);
            return Ok(result);
        }
        [HttpGet("contest/standing/{contestId}")]
        public async Task<IActionResult> GetStanding([FromRoute] string contestId)
        {
            var result = await _codeforcesRepository.GetContestStanding(contestId);
            return Ok(result);
        }
    }
}
