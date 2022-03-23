using ICPC_WebSite_Backend.Models.DTO;
using ICPC_WebSite_Backend.Repository;
using ICPC_WebSite_Backend.Utility;
using Microsoft.AspNetCore.Authorization;
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
     //   [Authorize]
        [HttpPost("registerCommunity")]
        public async Task<IActionResult> Register([FromBody] CommunityDTO community) {
            var validate = Validate.IsValidCommunity(community);
            if (!validate.Succeeded)
                return BadRequest(validate.Errors);

            var result = await _communityRepository.RegisterCommunityAsync(community);
            if (!result.Succeeded) {
                return Unauthorized(result.Errors);
            }
            return Ok(result);
        }
        [HttpGet("")]
        public async Task<IActionResult> GetAllCommunities() {
            var result = await _communityRepository.GetAllCommunities();
            if (!result.Succeeded) {
                return Unauthorized(result.Errors);
            }
            return Ok(result.Data);
        }
        [HttpPut("Approve")]
        public async Task<IActionResult> ApproveCommunity([FromQuery] int id) {
            var result = await _communityRepository.AcceptCommunity(id);
            if (!result.Succeeded) {
                return Unauthorized(result.Errors);
            }
            return Ok(result);
        }
        [HttpDelete("Reject")]
        public async Task<IActionResult> RejectCommunity([FromQuery] int id) {
            var result = await _communityRepository.RejectCommunity(id);
            if (!result.Succeeded) {
                return Unauthorized(result.Errors);
            }
            return Ok(result);
        }
    }
}
