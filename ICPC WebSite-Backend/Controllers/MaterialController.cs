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
    public class MaterialController : ControllerBase
    {
        private readonly IMatirialRepository _matirialRepository;

        public MaterialController(IMatirialRepository matirialRepository) {
            _matirialRepository = matirialRepository;
        }
        [HttpPost("addmatirial")]
        public async Task<IActionResult> addMaterial(MatirialDTO matirialDTO) {
            var validate = Validate.IsValidMaterinal(matirialDTO.Description);
            if (!validate.Succeeded)
                return BadRequest(validate);
            var result = await _matirialRepository.addMatirial(matirialDTO );
            if (!result.Succeeded) {
                return Unauthorized(result.Errors);
            }
            return Ok(result);
        }
        [HttpDelete("deletematirial")]
        public async Task<IActionResult> deleteMaterial(int  matirialId) {
            var result = await _matirialRepository.deleteMatiral(matirialId);
            if (!result.Succeeded) {
                return NotFound(result.Errors);
            }
            return Ok(result);
        }
        [HttpGet("getweekmaterials")]
        public async Task<IActionResult> getWeekMaterials(int weekId) {
            var result = await _matirialRepository.getWeekMaterials(weekId);
            if (!result.Succeeded) {
                return NotFound(result.Errors);
            }
            return Ok(result);
        }

    }
}
