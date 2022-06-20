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
        public async Task<IActionResult> AddMaterial(MaterialDto materialDto) {
            var validate = Validate.IsValidMaterial(materialDto.Description);
            if (!validate.Succeeded)
                return BadRequest(validate);
            var result = await _materialRepository.AddMaterial(materialDto );
            if (!result.Succeeded) {
                return Unauthorized(result.Errors);
            }
            return Ok(result);
        }
        [HttpDelete("deleteMaterial")]
        public async Task<IActionResult> DeleteMaterial(int  materialId) {
            var result = await _materialRepository.DeleteMaterial(materialId);
            if (!result.Succeeded) {
                return NotFound(result.Errors);
            }
            return Ok(result);
        }
        [HttpGet("getweekmaterials")]
        public async Task<IActionResult> GetWeekMaterials(int weekId) {
            var result = await _materialRepository.GetWeekMaterials(weekId);
            if (!result.Succeeded) {
                return NotFound(result.Errors);
            }
            return Ok(result);
        }

    }
}
