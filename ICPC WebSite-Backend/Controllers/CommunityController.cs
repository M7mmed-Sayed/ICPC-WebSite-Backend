using ICPC_WebSite_Backend.Models.DTO;
using ICPC_WebSite_Backend.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ICPC_WebSite_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommunityController : ControllerBase
    {
        private readonly ICommunityRepository _communityRepository;

        public CommunityController(ICommunityRepository communityRepository) {
            _communityRepository = communityRepository;
        }
        [HttpPost("registerCommunity")]
        public async Task<IActionResult> Register([FromBody] CommunityDTO community) {

            var result = await _communityRepository.RegisterCommunityAsync(community);
            if (!result.Succeeded) {
                return Unauthorized(result.Errors);
            }
            return Ok(result);
        }
    }
}
