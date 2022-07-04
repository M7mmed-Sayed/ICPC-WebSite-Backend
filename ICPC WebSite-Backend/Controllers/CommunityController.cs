using ICPC_WebSite_Backend.Data;
using ICPC_WebSite_Backend.Repository;
using ICPC_WebSite_Backend.Security;
using ICPC_WebSite_Backend.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UtilityLibrary.ModelsDTO;
using UtilityLibrary.Utility;

namespace ICPC_WebSite_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommunityController : ControllerBase
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly ICommunityRepository _communityRepository;
        private readonly ApplicationDbContext _applicationDbContext;

        public CommunityController(ICommunityRepository communityRepository, IAuthorizationService authorizationService,ApplicationDbContext applicationDbContext)
        {
            _authorizationService = authorizationService;
            _communityRepository = communityRepository;
            _applicationDbContext = applicationDbContext;
        }
        [NonAction]
        private async Task<int> GetCommunityId(string userId)
        {
            var user = await _applicationDbContext.Users.FindAsync(userId);
            return user?.CommunityId ?? 0;
        }
        [NonAction]
        private async Task<bool> IsAuthorized(int communityId)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, new ClaimResource(ClaimsNames.CommunityIdClaimName, communityId), "EditAccess");
            return authorizationResult.Succeeded;
        }
        /// <summary>
        /// register a community in the website
        /// </summary>
        /// <param name="community">community data</param>
        /// <returns>returns the created community</returns>
        [Authorize(Roles = RolesList.Administrator)]
        [HttpPost("registerCommunity")]
        public async Task<IActionResult> Register([FromBody] CommunityDto community) {
            var validate = Validate.IsValidCommunity(community);
            if (!validate.Succeeded)
                return BadRequest(validate);

            var result = await _communityRepository.RegisterCommunityAsync(community);
            if (!result.Succeeded) {
                return BadRequest(result);
            }
            return Created(result.Data.Id.ToString(),result);
        }
        /// <summary>
        /// edit community data
        /// </summary>
        /// <param name="communityId">the id of the community</param>
        /// <param name="community">current data of the community</param>
        /// <returns>returns failed or succeeded</returns>
        [Authorize(Roles = RolesList.Administrator + "," + RolesList.CommunityLeader)]
        [HttpPut("editCommunity")]
        public async Task<IActionResult> EditCommunity(int communityId,[FromBody] CommunityDto community) {
            if (!await IsAuthorized(communityId)) return Forbid();
            var validate = Validate.IsValidCommunity(community);
            if (!validate.Succeeded)
                return BadRequest(validate);

            var result = await _communityRepository.EditCommunity(communityId,community);
            if (!result.Succeeded) {
                return BadRequest(result);
            }
            return Ok(result);
        }
        /// <summary>
        /// delete a specific communicty
        /// </summary>
        /// <param name="communityId">the id of the community</param>
        /// <returns>returns failed or succeeded</returns>
        [Authorize(Roles = RolesList.Administrator)]
        [HttpDelete("deleteCommunity")]
        public async Task<IActionResult> DeleteCommunity(int communityId) {
            if (!await IsAuthorized(communityId)) return Forbid();
            var result = await _communityRepository.DeleteCommunity(communityId);
            if (!result.Succeeded) {
                return BadRequest(result);
            }
            return Ok(result);
        }
        /// <summary>
        /// get all communities exist in the website
        /// </summary>
        /// <returns>return list of communities data</returns>
        [HttpGet("")]
        public async Task<IActionResult> GetAllCommunities() {
            var result = await _communityRepository.GetAllCommunities();
            if (!result.Succeeded) {
                return BadRequest(result);
            }
            return Ok(result);
        }
        /// <summary>
        /// get a community
        /// </summary>
        /// <param name="id">the id of the community</param>
        /// <returns>returns the community data if exist otherwise returns an error</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCommunities(int id) {
            var result = await _communityRepository.GetCommunity(id);
            if (!result.Succeeded) {
                return NotFound(result);
            }
            return Ok(result);
        }
        /// <summary>
        /// assign a role for a specific user in the community with communityId
        /// </summary>
        /// <param name="userId">the id of the uesr</param>
        /// <param name="communityId">the id of the community</param>
        /// <param name="roleName">the name of the rule</param>
        /// <returns>returns failed or succeeded</returns>
        [Authorize(Roles = RolesList.Administrator + "," + RolesList.CommunityLeader)]
        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole([FromQuery] string userId, [FromQuery] int communityId, [FromQuery] string roleName)
        {
            if (!await IsAuthorized(communityId)) return Forbid();
            var result = await _communityRepository.AssignRole(userId, communityId, roleName);
            if (!result.Succeeded)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [Authorize(Roles = RolesList.Administrator + "," + RolesList.CommunityLeader)]
        [HttpDelete("removeRole")]
        public async Task<IActionResult> RemovveRole([FromQuery] string userId, [FromQuery] int communityId, [FromQuery] string roleName)
        {
            if (!await IsAuthorized(communityId)) return Forbid();
            var result = await _communityRepository.RemoveRole(userId, communityId, roleName);
            if (!result.Succeeded)
            {
                return Unauthorized(result);
            }
            return Ok(result);
        }
        [Authorize(Roles = RolesList.Administrator + "," + RolesList.CommunityLeader)]
        [HttpDelete("kickuser")]
        public async Task<IActionResult> KickUserOut([FromQuery] string userId)
        {
            var communityId = await GetCommunityId(userId);
            if (!await IsAuthorized(communityId)) return Forbid();
            var result = await _communityRepository.KickUser(userId);
            if (!result.Succeeded)
            {
                return Unauthorized(result);
            }
            return Ok(result);
        }
        /// <summary>
        /// make a request from specific user to join specific community
        /// </summary>
        /// <param name="userId">the id of the user</param>
        /// <param name="communityId">the id of the community</param>
        /// <returns>returns failed or succeeded</returns>
        [Authorize]
        [HttpPost("Join/{communityId}")]
        public async Task<IActionResult> JoinRequest([FromQuery] string userId, int communityId) {
            var result = await _communityRepository.JoinRequest(userId, communityId);
            if (!result.Succeeded) {
                return BadRequest(result);
            }
            return Ok(result);
        }
        /// <summary>
        /// get all members that make a request for a specific member
        /// </summary>
        /// <param name="communityId">the id of the community</param>
        /// <returns>return list of community members</returns>
        [Authorize(Roles = RolesList.Administrator + "," + RolesList.CommunityLeader)]
        [HttpGet("Requests/{communityId}")]
        public async Task<IActionResult> GetRequest(int communityId) {
            if (!await IsAuthorized(communityId)) return Forbid();
            var result = await _communityRepository.GetRequest(communityId);
            if (!result.Succeeded) {
                return BadRequest(result);
            }
            return Ok(result);
        }
        /// <summary>
        /// accept of reject a joining request for a community
        /// </summary>
        /// <param name="userId">the id of the user who made a request</param>
        /// <param name="communityId">the community which the user want to join</param>
        /// <param name="approve">bool variable to determine accept the request or not</param>
        /// <returns>returns failed or succeeded</returns>
        [Authorize(Roles = RolesList.Administrator + "," + RolesList.CommunityLeader)]
        [HttpPost("RequestRespond/{communityId}")]
        public async Task<IActionResult> ApproveRequest([FromQuery] string userId, int communityId, [FromQuery] bool approve)
        {
            if (!await IsAuthorized(communityId)) return Forbid();
            var result = await _communityRepository.ResponseToRequest(userId, communityId, approve);
            if (!result.Succeeded) {
                return BadRequest(result);
            }
            return Ok(result);
        }
        /// <summary>
        /// get all members of a specific community
        /// </summary>
        /// <param name="communityId">the id of the community</param>
        /// <returns>returns list of all members in the community</returns>
        [HttpGet("Members/{communityId}")]
        public async Task<IActionResult> GetMembers(int communityId) {
            var result = await _communityRepository.GetMembers(communityId);
            if (!result.Succeeded) {
                return BadRequest(result);
            }
            return Ok(result);
        }
        /// <summary>
        /// count the number of members in the community
        /// </summary>
        /// <param name="communityId">the id of the community</param>
        /// <returns>return an integer represents the number of members</returns>
        [HttpGet("CountMembers/{communityId}")]
        public async Task<IActionResult> CountMembers(int communityId) {
            var result = await _communityRepository.CountMembers(communityId);
            if (!result.Succeeded) {
                return BadRequest(result);
            }
            return Ok(result);
        }

    }
}
