using ICPC_WebSite_Backend.Data.Models.DTO;
using ICPC_WebSite_Backend.Repository;
using ICPC_WebSite_Backend.Security;
using ICPC_WebSite_Backend.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ICPC_WebSite_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommunityController : ControllerBase
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly ICommunityRepository _communityRepository;

        public CommunityController(ICommunityRepository communityRepository, IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
            _communityRepository = communityRepository;
        }

        [Authorize]
        [HttpGet("IsAuthorized")]
        public async Task<bool> IsAuthorized(int communityId)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, new ClaimResource(ClaimsNames.CommunityIdClaimName, communityId), "EditAccess");
            return authorizationResult.Succeeded;
        }
        [Authorize]
        [HttpPost("registerCommunity")]
        public async Task<IActionResult> Register([FromBody] CommunityDto community) {
            var validate = Validate.IsValidCommunity(community);
            if (!validate.Succeeded)
                return BadRequest(validate);

            var result = await _communityRepository.RegisterCommunityAsync(community);
            if (!result.Succeeded) {
                return Unauthorized(result);
            }
            return Ok(result);
        }
        [Authorize]
        [HttpPost("editCommunity")]
        public async Task<IActionResult> EditCommunity(int communityId,[FromBody] CommunityDto community) {
            var validate = Validate.IsValidCommunity(community);
            if (!validate.Succeeded)
                return BadRequest(validate);

            var result = await _communityRepository.EditCommunity(communityId,community);
            if (!result.Succeeded) {
                return Unauthorized(result);
            }
            return Ok(result);
        }
        [Authorize]
        [HttpPost("deleteCommunity")]
        public async Task<IActionResult> DeleteCommunity(int communityId) {
            var result = await _communityRepository.DeleteCommunity(communityId);
            if (!result.Succeeded) {
                return Unauthorized(result);
            }
            return Ok(result);
        }
        [HttpGet("")]
        public async Task<IActionResult> GetAllCommunities() {
            var result = await _communityRepository.GetAllCommunities();
            if (!result.Succeeded) {
                return Unauthorized(result);
            }
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCommunities(int id) {
            var result = await _communityRepository.GetCommunity(id);
            if (!result.Succeeded) {
                return Unauthorized(result);
            }
            return Ok(result);
        }

        [Authorize(Roles = RolesList.CommunityLeader)]
        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole([FromQuery] string userId, [FromQuery] int communityId, [FromQuery] string roleName)
        {
            if (!await IsAuthorized(communityId)) return Forbid();
            var result = await _communityRepository.AssignRole(userId, communityId, roleName);
            if (!result.Succeeded)
            {
                return Unauthorized(result);
            }
            return Ok(result);
        }
        [Authorize]
        [HttpPost("Join/{communityId}")]
        public async Task<IActionResult> JoinRequest([FromQuery] string userId, int communityId) {
            var result = await _communityRepository.JoinRequest(userId, communityId);
            if (!result.Succeeded) {
                return Unauthorized(result);
            }
            return Ok(result);
        }
        [Authorize(Roles = RolesList.CommunityLeader)]
        [HttpPost("Requests/{communityId}")]
        public async Task<IActionResult> GetRequest(int communityId) {
            var result = await _communityRepository.GetRequest(communityId);
            if (!result.Succeeded) {
                return Unauthorized(result);
            }
            return Ok(result);
        }
        [Authorize(Roles = RolesList.CommunityLeader)]
        [HttpPost("RequestRespond/{communityId}")]
        public async Task<IActionResult> ApproveRequest([FromQuery] string userId, int communityId, [FromQuery] bool approve)
        {
            if (!await IsAuthorized(communityId)) return Forbid();
            var result = await _communityRepository.ResponseToRequest(userId, communityId, approve);
            if (!result.Succeeded) {
                return Unauthorized(result);
            }
            return Ok(result);
        }
        [HttpGet("Members/{communityId}")]
        public async Task<IActionResult> GetMembers(int communityId) {
            var result = await _communityRepository.GetMembers(communityId);
            if (!result.Succeeded) {
                return Unauthorized(result);
            }
            return Ok(result);
        }
        [HttpGet("CountMembers/{communityId}")]
        public async Task<IActionResult> CountMembers(int communityId) {
            var result = await _communityRepository.CountMembers(communityId);
            if (!result.Succeeded) {
                return Unauthorized(result);
            }
            return Ok(result);
        }
        [HttpGet("trainings/{communityId}")]
        public async Task<IActionResult> GetTrainingByCommunity(int communityId) {
            var result = await _communityRepository.GetTrainingsAtCommunity(communityId);
            if (!result.Succeeded) {
                return Unauthorized(result);
            }
            return Ok(result);
        }
    }
}
