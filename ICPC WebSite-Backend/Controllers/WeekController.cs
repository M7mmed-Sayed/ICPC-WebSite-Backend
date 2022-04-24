using ICPC_WebSite_Backend.Data.Models.DTO;
using ICPC_WebSite_Backend.Models.DTO;
using ICPC_WebSite_Backend.Repository;
using ICPC_WebSite_Backend.Utility;
using Microsoft.AspNetCore.Http;
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
        public async Task<IActionResult> addWeek(WeekDTO weekDTO) {
            if (string.IsNullOrEmpty(weekDTO.Name)) {
                return BadRequest("Invalid Name");
            }
            var result = await _weekRepository.AddWeek(weekDTO);
            if (!result.Succeeded) {
                return Unauthorized(result.Errors);
            }
            return Ok(result);
        }
        [HttpPut("updateweek")]
        public async Task<IActionResult> updateWeek(int weekId,WeekDTO weekDTO) {
            if (string.IsNullOrEmpty(weekDTO.Name)) {
                return BadRequest("Invalid Name");
            }
            var result = await _weekRepository.UpdateWeek(weekId,weekDTO);
            if (!result.Succeeded) {
                return Unauthorized(result.Errors);
            }
            return Ok(result);
        }
        [HttpGet("templateweeks")]
        public async Task<IActionResult> templateWeeks() {
            var result = await _weekRepository.GetAllTemplateWeeks();
            if (!result.Succeeded) {
                return Unauthorized(result.Errors);
            }
            return Ok(result);
        }
        [HttpGet("allweeks")]
        public async Task<IActionResult> getAllWeeks() {
            var result = await _weekRepository.GetAllWeeks();
            if (!result.Succeeded) {
                return Unauthorized(result.Errors);
            }
            return Ok(result);
        }
        [HttpGet("getweek")]
        public async Task<IActionResult> getWeek(int weekId) {
            var result = await _weekRepository.GetTheWeek(weekId);
            if (!result.Succeeded) {
                return Unauthorized(result.Errors);
            }
            return Ok(result);
        }
        [HttpPost("createemplateweek")]
        public async Task<IActionResult> creaTemplateWeek(int weekId) {
            var result = await _weekRepository.createTemplateWeek(weekId);
            if (!result.Succeeded) {
                return Unauthorized(result.Errors);
            }
            return Ok(result);
        }



    }
}
