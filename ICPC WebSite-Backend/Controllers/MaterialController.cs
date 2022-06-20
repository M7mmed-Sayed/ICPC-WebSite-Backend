using ICPC_WebSite_Backend.Data.Models.DTO;
using ICPC_WebSite_Backend.Repository;
using ICPC_WebSite_Backend.Utility;
using Microsoft.AspNetCore.Mvc;

namespace ICPC_WebSite_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialController : ControllerBase
    {
        private readonly IMaterialRepository _materialRepository;

        public MaterialController(IMaterialRepository materialRepository) {
            _materialRepository = materialRepository;
        }
        [HttpPost("addMaterial")]
        public async Task<IActionResult> addMaterial(MaterialDTO MaterialDTO) {
            var validate = Validate.IsValidMaterial(MaterialDTO.Description);
            if (!validate.Succeeded)
                return BadRequest(validate);
            var result = await _materialRepository.addMaterial(MaterialDTO );
            if (!result.Succeeded) {
                return Unauthorized(result.Errors);
            }
            return Ok(result);
        }
        [HttpDelete("deleteMaterial")]
        public async Task<IActionResult> deleteMaterial(int  MaterialId) {
            var result = await _materialRepository.deleteMaterial(MaterialId);
            if (!result.Succeeded) {
                return NotFound(result.Errors);
            }
            return Ok(result);
        }
        [HttpGet("getweekmaterials")]
        public async Task<IActionResult> getWeekMaterials(int weekId) {
            var result = await _materialRepository.getWeekMaterials(weekId);
            if (!result.Succeeded) {
                return NotFound(result.Errors);
            }
            return Ok(result);
        }

    }
}
