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

        /// <summary>
        /// add a week to a community
        /// </summary>
        /// <param name="weekDto">contains week important data like week id and community id</param>
        /// <returns>failed or succeeded</returns>
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

            return Created(result.Data.Id.ToString(),result);
        }

        /// <summary>
        /// update a week with new data
        /// </summary>
        /// <param name="weekId">id the week</param>
        /// <param name="weekDto">the new data</param>
        /// <returns>failed or succeeded</returns>
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

        /// <summary>
        /// delete a specific week
        /// </summary>
        /// <param name="weekId">id of the week</param>
        /// <returns>succeeded or failed</returns>
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

        /// <summary>
        /// get all week inside a community
        /// </summary>
        /// <param name="communityId">id of the community</param>
        /// <returns>if succeeded return list of week otherwise returns an error</returns>
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

        /// <summary>
        /// get all week inside a training
        /// </summary>
        /// <param name="trainingId">id of the training</param>
        /// <returns>if succeeded return list of week otherwise returns an error</returns>
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

        /// <summary>
        /// get a specific week
        /// </summary>
        /// <param name="weekId">id of the  week</param>
        /// <returns>if succeeded returns week data otherwise returns an error</returns>
        [HttpGet("getweek")]
        public async Task<IActionResult> GetWeek(int weekId)
        {
            var result = await _weekRepository.GetTheWeek(weekId);
            if (!result.Succeeded)
            {
                return NotFound(result.Errors);
            }

            return Ok(result);
        }

        /// <summary>
        /// link a sheet to a week
        /// </summary>
        /// <param name="weekId">id of the week</param>
        /// <param name="sheetId">id of the sheet</param>
        /// <returns>succeeded or failed</returns>
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

        /// <summary>
        /// unlink a sheet from a week
        /// </summary>
        /// <param name="weekId">id of the week</param>
        /// <param name="sheetId">id of the sheet</param>
        /// <returns>succeeded or failed</returns>
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