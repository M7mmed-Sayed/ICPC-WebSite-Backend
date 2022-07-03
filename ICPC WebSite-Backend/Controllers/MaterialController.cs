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
    public class MaterialController : ControllerBase
    {
        private readonly IMaterialRepository _materialRepository;
        private readonly IAuthorizationService _authorizationService;
        private readonly ApplicationDbContext _applicationDbContext;

        public MaterialController(IMaterialRepository materialRepository, IAuthorizationService authorizationService, ApplicationDbContext applicationDbContext)
        {
            _materialRepository = materialRepository;
            _authorizationService = authorizationService;
            _applicationDbContext = applicationDbContext;
        }

        [NonAction]
        private async Task<int> GetCommunityId(int id, bool isMaterial)
        {
            var weekId = id;
            if (isMaterial)
            {
                var material = await _applicationDbContext.Materials.FindAsync(id);
                if (material == null) return 0;
                weekId = material.WeekId;
            }

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
        [HttpPost("addMaterial")]
        public async Task<IActionResult> AddMaterial(MaterialDto materialDto)
        {
            if (!await IsAuthorized(await GetCommunityId(materialDto.WeekId,false))) return Forbid();

            var validate = Validate.IsValidMaterial(materialDto.Description);
            if (!validate.Succeeded)
                return BadRequest(validate);
            var result = await _materialRepository.AddMaterial(materialDto);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(result);
        }
        [Authorize(Roles = RolesList.Administrator + "," + RolesList.CommunityLeader + "," + RolesList.HeadOfTraining)]
        [HttpDelete("deleteMaterial")]
        public async Task<IActionResult> DeleteMaterial(int materialId)
        {
            if (!await IsAuthorized(await GetCommunityId(materialId,true))) return Forbid();
            var result = await _materialRepository.DeleteMaterial(materialId);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(result);
        }

        [HttpGet("getweekmaterials")]
        public async Task<IActionResult> GetWeekMaterials(int weekId)
        {
            var result = await _materialRepository.GetWeekMaterials(weekId);
            if (!result.Succeeded)
            {
                return NotFound(result.Errors);
            }

            return Ok(result);
        }
    }
}