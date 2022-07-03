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
    public class TrainingController : ControllerBase
    {
        private readonly ITrainingRepository _trainingRepository;
        private readonly IAuthorizationService _authorizationService;
        private readonly ApplicationDbContext _applicationDbContext;

        public TrainingController(ITrainingRepository trainingRepository, IAuthorizationService authorizationService,ApplicationDbContext applicationDbContext)
        {
            _trainingRepository = trainingRepository;
            _authorizationService = authorizationService;
            _applicationDbContext = applicationDbContext;
        }
        [HttpGet]
        public async Task<int> GetCommunityId(int trainingId)
        {
            var training = await _applicationDbContext.Trainings.FindAsync(trainingId);
            return training?.CommunityId ?? 0;
        }
        [Authorize]
        [HttpGet("IsAuthorized")]
        public async Task<bool> IsAuthorized(int communityId)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, new ClaimResource(ClaimsNames.CommunityIdClaimName, communityId), "EditAccess");
            return authorizationResult.Succeeded;
        }

        [Authorize(Roles = RolesList.Administrator + "," + RolesList.CommunityLeader + "," + RolesList.HeadOfTraining)]
        [HttpPost("addtraining")]
        public async Task<IActionResult> addtraining(TrainingDTO trainingDTO)
        {
            if (!await IsAuthorized(trainingDTO.Community_Id)) return Forbid();

            var validate = Validate.IsValidTraining(trainingDTO);
            if (!validate.Succeeded)
                return BadRequest(validate);
            var result = await _trainingRepository.AddTraining(trainingDTO);
            if (!result.Succeeded)
            {
                return Unauthorized(result.Errors);
            }

            return Ok(result);
        }
        [Authorize(Roles = RolesList.Administrator + "," + RolesList.CommunityLeader + "," + RolesList.HeadOfTraining)]
        [HttpPut("updatetraining")]
        public async Task<IActionResult> updatetraining(int trainingId, TrainingDTO trainingDTO) {
            
            if (!await IsAuthorized(await GetCommunityId(trainingId))) return Forbid();

            var validate = Validate.IsValidTraining(trainingDTO);
            if (!validate.Succeeded)
                return BadRequest(validate);
            var result = await _trainingRepository.UpdateTraining(trainingId, trainingDTO);
            if (!result.Succeeded) {
                return Unauthorized(result.Errors);
            }
            return Ok(result);
        }
        [HttpGet("alltrainings")]
        public async Task<IActionResult> getAlltrainings([FromQuery] int communityId) {
            var result = await _trainingRepository.GetAllTrainings(communityId);
            if (!result.Succeeded) {
                return Unauthorized(result.Errors);
            }
            return Ok(result);
        }
        [HttpGet("gettraining")]
        public async Task<IActionResult> getTraining(int trainingId) {
            var result = await _trainingRepository.GetTraining(trainingId);
            if (!result.Succeeded) {
                return Unauthorized(result.Errors);
            }
            return Ok(result);
        }
        [Authorize(Roles = RolesList.Administrator + "," + RolesList.CommunityLeader + "," + RolesList.HeadOfTraining)]

        [HttpDelete("deletetraining")]
        public async Task<IActionResult> deletetraining(int trainingId) {
            if (!await IsAuthorized(await GetCommunityId(trainingId))) return Forbid();

            var result = await _trainingRepository.DeleteTraining(trainingId);
            if (!result.Succeeded) {
                return NotFound(result.Errors);
            }
            return Ok(result);
        }
        [Authorize(Roles = RolesList.Administrator + "," + RolesList.CommunityLeader + "," + RolesList.HeadOfTraining)]

        [HttpPost("linkweek")]
        public async Task<IActionResult> LinkWeek(int trainingId, int weekId)
        {
            if (!await IsAuthorized(await GetCommunityId(trainingId))) return Forbid();

            var result = await _trainingRepository.LinkWeek(trainingId, weekId);
            if (!result.Succeeded) {
                return NotFound(result.Errors);
            }
            return Ok(result);
        }
        [Authorize(Roles = RolesList.Administrator + "," + RolesList.CommunityLeader + "," + RolesList.HeadOfTraining)]
        [HttpDelete("Unlinkweek")]
        public async Task<IActionResult> UnLinkWeek(int trainingId, int weekId)
        {
            if (!await IsAuthorized(await GetCommunityId(trainingId))) return Forbid();

            var result = await _trainingRepository.UnLinkWeek(trainingId, weekId);
            if (!result.Succeeded) {
                return NotFound(result.Errors);
            }
            return Ok(result);
        }
        [Authorize]
        [HttpPost("Join/{trainingId}")]
        public async Task<IActionResult> JoinTraining([FromQuery] string userId, int trainingId) {
            var result = await _trainingRepository.JoinTraining(userId, trainingId);
            if (!result.Succeeded) {
                return Unauthorized(result);
            }
            return Ok(result);
        }
        [Authorize(Roles = RolesList.Administrator + "," + RolesList.CommunityLeader + "," + RolesList.HeadOfTraining)]
        [HttpPost("RequestRespond/{trainingId}")]
        public async Task<IActionResult> ApproveJoinTrainingRequest([FromQuery] string userId, int trainingId, [FromQuery] bool approve) {
            if (!await IsAuthorized(await GetCommunityId(trainingId))) return Forbid();

            var result = await _trainingRepository.ResponseToTrainingRequest(userId, trainingId, approve);
            if (!result.Succeeded) {
                return Unauthorized(result);
            }
            return Ok(result);
        }
        [Authorize]
        [HttpGet("Members/{trainingId}")]
        public async Task<IActionResult> GetTrainingMembers(int trainingId, [FromQuery] string status)
        {
            if (status == ConstVariable.PendingStatus)
            {
                if (!await IsAuthorized(await GetCommunityId(trainingId))) return Forbid();
            }

            var result = await _trainingRepository.GetTrainingMembersAsync(trainingId, status);
            if (!result.Succeeded)
            {
                return Unauthorized(result);
            }

            return Ok(result);
        }
    }
}
