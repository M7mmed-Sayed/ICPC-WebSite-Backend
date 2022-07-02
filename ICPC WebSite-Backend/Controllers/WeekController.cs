using ICPC_WebSite_Backend.Repository;
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

        public WeekController(IWeekRepository weekRepository) {
            _weekRepository = weekRepository;
        }
        [HttpPost("addweek")]
        public async Task<IActionResult> AddWeek(WeekDto weekDto) {
            var validate = Validate.IsValidWeek(weekDto);
            if (!validate.Succeeded)
                return BadRequest(validate);
            var result = await _weekRepository.AddWeek(weekDto);
            if (!result.Succeeded) {
                return Unauthorized(result.Errors);
            }
            return Ok(result);
        }
        [HttpPut("updateweek")]
        public async Task<IActionResult> UpdateWeek(int weekId,WeekDto weekDto) {
            var validate = Validate.IsValidWeek(weekDto);
            if (!validate.Succeeded)
                return BadRequest(validate);
            var result = await _weekRepository.UpdateWeek(weekId,weekDto);
            if (!result.Succeeded) {
                return Unauthorized(result.Errors);
            }
            return Ok(result);
        }
        [HttpDelete("deleteweek")]
        public async Task<IActionResult> deleteWeek(int weekId) {
            var result = await _weekRepository.deleteWeek(weekId);
            if (!result.Succeeded) {
                return NotFound(result.Errors);
            }
            return Ok(result);
        }
        
        [HttpGet("allweeksbycommunity")]
        public async Task<IActionResult> GetWeeksByCommunity(int communityId) {
            var result = await _weekRepository.GetWeeksByCommunity(communityId);
            if (!result.Succeeded) {
                return Unauthorized(result.Errors);
            }
            return Ok(result);
        }
        [HttpGet("allweeksbytraining")]
        public async Task<IActionResult> GetWeeksByTraining(int trainingId) {
            var result = await _weekRepository.GetWeeksByTraining(trainingId);
            if (!result.Succeeded) {
                return Unauthorized(result.Errors);
            }
            return Ok(result);
        }
        [HttpGet("getweek")]
        public async Task<IActionResult> GetWeek(int weekId) {
            var result = await _weekRepository.GetTheWeek(weekId);
            if (!result.Succeeded) {
                return Unauthorized(result.Errors);
            }
            return Ok(result);
        }
       
        [HttpPost("linksheet")]
        public async Task<IActionResult> LinkSheet(int weekId, int sheetId)
        {
            var result = await _weekRepository.LinkSheet(weekId, sheetId);
            if (!result.Succeeded) {
                return NotFound(result.Errors);
            }
            return Ok(result);
        }
        [HttpDelete("unlinksheet")]
        public async Task<IActionResult> UnLinkSheet(int weekId, int sheetId)
        {
            var result = await _weekRepository.UnLinkSheet(weekId, sheetId);
            if (!result.Succeeded) {
                return NotFound(result.Errors);
            }
            return Ok(result);
        }
        



    }
}
