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
    public class WeekController : ControllerBase
    {
        private readonly IWeekRepository _weekRepository;
        private readonly IAuthorizationService _authorizationService;
        private readonly ApplicationDbContext _applicationDbContext;

        public WeekController(IWeekRepository weekRepository, IAuthorizationService authorizationService, ApplicationDbContext applicationDbContext)
        {
            _weekRepository = weekRepository;
            _authorizationService = authorizationService;
            _applicationDbContext = applicationDbContext;
        }

        [NonAction]
        private async Task<int> GetCommunityId(int weekId)
        {
            var week = await _applicationDbContext.Weeks.FindAsync(weekId);
            return week?.CommunityId ?? 0;
        }

        [NonAction]
        private async Task<bool> IsAuthorized(int communityId)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, new ClaimResource(ClaimsNames.CommunityIdClaimName, communityId), "EditAccess");
            return authorizationResult.Succeeded;
        }

        [Authorize(Roles = RolesList.Administrator + "," + RolesList.CommunityLeader + "," + RolesList.HeadOfTraining)]
        [HttpPost("addweek")]
        public async Task<IActionResult> AddWeek(WeekDto weekDto)
        {
            if (!await IsAuthorized(weekDto.CommunityId)) return Forbid();

            var validate = Validate.IsValidWeek(weekDto);
            if (!validate.Succeeded)
                return BadRequest(validate);
            var result = await _weekRepository.AddWeek(weekDto);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(result);
        }

        [Authorize(Roles = RolesList.Administrator + "," + RolesList.CommunityLeader + "," + RolesList.HeadOfTraining)]
        [HttpPut("updateweek")]
        public async Task<IActionResult> UpdateWeek(int weekId, WeekDto weekDto)
        {
            if (!await IsAuthorized(weekDto.CommunityId)) return Forbid();
            var validate = Validate.IsValidWeek(weekDto);
            if (!validate.Succeeded)
                return BadRequest(validate);
            var result = await _weekRepository.UpdateWeek(weekId, weekDto);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(result);
        }

        [Authorize(Roles = RolesList.Administrator + "," + RolesList.CommunityLeader + "," + RolesList.HeadOfTraining)]
        [HttpDelete("deleteweek")]
        public async Task<IActionResult> deleteWeek(int weekId)
        {
            if (!await IsAuthorized(await GetCommunityId(weekId))) return Forbid();
            var result = await _weekRepository.deleteWeek(weekId);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(result);
        }

        [HttpGet("allweeksbycommunity")]
        public async Task<IActionResult> GetWeeksByCommunity(int communityId)
        {
            var result = await _weekRepository.GetWeeksByCommunity(communityId);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(result);
        }

        [HttpGet("allweeksbytraining")]
        public async Task<IActionResult> GetWeeksByTraining(int trainingId)
        {
            var result = await _weekRepository.GetWeeksByTraining(trainingId);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(result);
        }

        [HttpGet("getweek")]
        public async Task<IActionResult> GetWeek(int weekId)
        {
            var result = await _weekRepository.GetTheWeek(weekId);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(result);
        }

        [Authorize(Roles = RolesList.Administrator + "," + RolesList.CommunityLeader + "," + RolesList.HeadOfTraining)]
        [HttpPost("linksheet")]
        public async Task<IActionResult> LinkSheet(int weekId, int sheetId)
        {
            if (!await IsAuthorized(await GetCommunityId(weekId))) return Forbid();

            var result = await _weekRepository.LinkSheet(weekId, sheetId);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(result);
        }

        [Authorize(Roles = RolesList.Administrator + "," + RolesList.CommunityLeader + "," + RolesList.HeadOfTraining)]
        [HttpDelete("unlinksheet")]
        public async Task<IActionResult> UnLinkSheet(int weekId, int sheetId)
        {
            if (!await IsAuthorized(await GetCommunityId(weekId))) return Forbid();
            var result = await _weekRepository.UnLinkSheet(weekId, sheetId);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(result);
        }
    }
}