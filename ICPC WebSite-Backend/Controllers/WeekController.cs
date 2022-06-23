using ICPC_WebSite_Backend.Data.Models.DTO;
using ICPC_WebSite_Backend.Repository;
using ICPC_WebSite_Backend.Utility;
using Microsoft.AspNetCore.Mvc;

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
        [HttpGet("templateweeks")]
        public async Task<IActionResult> TemplateWeeks() {
            var result = await _weekRepository.GetAllTemplateWeeks();
            if (!result.Succeeded) {
                return Unauthorized(result.Errors);
            }
            return Ok(result);
        }
        [HttpGet("allweeks")]
        public async Task<IActionResult> GetAllWeeks() {
            var result = await _weekRepository.GetAllWeeks();
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
        [HttpPost("createemplateweek")]
        public async Task<IActionResult> CreaTemplateWeek(int weekId) {
            var result = await _weekRepository.CreateTemplateWeek(weekId);
            if (!result.Succeeded) {
                return Unauthorized(result.Errors);
            }
            return Ok(result);
        }



    }
}
