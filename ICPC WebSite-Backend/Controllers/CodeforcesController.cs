using ICPC_WebSite_Backend.Repository;
using Microsoft.AspNetCore.Http;
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
        [HttpGet("contest/{contestId}")]
        public async Task<IActionResult> getStanding([FromRoute]string contestId,string userCodeforcesHandle)
        {
            var result = await _codeforcesRepository.GetContestSubmissions(contestId, userCodeforcesHandle);
            return Ok(result);
        }
    }
}
