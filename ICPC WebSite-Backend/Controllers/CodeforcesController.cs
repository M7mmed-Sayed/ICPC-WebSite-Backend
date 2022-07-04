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
        /// <summary>
        /// get all the sumbimssions of some user from a contest on codeforces
        /// </summary>
        /// <param name="contestId">the id of the contest</param>
        /// <param name="userCodeforcesHandle">the handle of the user on codeforces</param>
        /// <returns>returns list of contest submissions respone</returns>
        [HttpGet("contest/submissions/{contestId}")]
        public async Task<IActionResult> GetSubmissions([FromRoute]string contestId,[FromQuery]string? userCodeforcesHandle)
        {
            var result = await _codeforcesRepository.GetContestSubmissions(contestId, userCodeforcesHandle);
            return Ok(result);
        }
        /// <summary>
        /// get the standing of a contest on codeforces
        /// </summary>
        /// <param name="contestId">the id of the contest</param>
        /// <returns>returns the standing of the contest</returns>
        [HttpGet("contest/standing/{contestId}")]
        public async Task<IActionResult> GetStanding([FromRoute] string contestId)
        {
            var result = await _codeforcesRepository.GetContestStanding(contestId);
            return Ok(result);
        }
    }
}
