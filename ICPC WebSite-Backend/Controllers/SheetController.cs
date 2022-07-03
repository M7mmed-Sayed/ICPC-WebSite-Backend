using ICPC_WebSite_Backend.Data;
using ICPC_WebSite_Backend.Repository;
using ICPC_WebSite_Backend.Security;
using ICPC_WebSite_Backend.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UtilityLibrary.ModelsDTO;
using UtilityLibrary.Utility;

namespace ICPC_WebSite_Backend.Controllers;

public class SheetController : Controller
{
    private readonly ISheetRepository _sheetRepository;
    private readonly IAuthorizationService _authorizationService;
    private readonly ApplicationDbContext _applicationDbContext;

    public SheetController(ISheetRepository sheetRepository, IAuthorizationService authorizationService, ApplicationDbContext applicationDbContext)
    {
        _sheetRepository = sheetRepository;
        _authorizationService = authorizationService;
        _applicationDbContext = applicationDbContext;
    }
    [NonAction]
    private async Task<int> GetCommunityId(int sheetId)
    {
        var sheet = await _applicationDbContext.Sheets.FindAsync(sheetId);
        return sheet?.CommunityId ?? 0;
    }
    [NonAction]
    private async Task<bool> IsAuthorized(int communityId)
    {
        var authorizationResult = await _authorizationService.AuthorizeAsync(User, new ClaimResource(ClaimsNames.CommunityIdClaimName, communityId), "EditAccess");
        return authorizationResult.Succeeded;
    }

    [Authorize(Roles = RolesList.Administrator + "," + RolesList.CommunityLeader + "," + RolesList.HeadOfTraining)]
    [HttpPost("addsheet")]
    public async Task<IActionResult> AddSheet([FromBody]SheetDto sheetDto) {
        if (!await IsAuthorized(sheetDto.CommunityId)) return Forbid();
        var result = await _sheetRepository.AddSheet(sheetDto);
        if (!result.Succeeded) {
            return BadRequest(result.Errors);
        }
        return Created(result.Data.Id.ToString(),result);
    }
    [Authorize(Roles = RolesList.Administrator + "," + RolesList.CommunityLeader + "," + RolesList.HeadOfTraining)]
    [HttpPut("updatesheet")]
    public async Task<IActionResult> Updatesheet(int sheetId,SheetDto sheetDto) {
        if (!await IsAuthorized(sheetDto.CommunityId)) return Forbid();
        var result = await _sheetRepository.UpdateSheet(sheetId,sheetDto);
        if (!result.Succeeded) {
            return BadRequest(result.Errors);
        }
        return Ok(result);
    }
    [Authorize(Roles = RolesList.Administrator + "," + RolesList.CommunityLeader + "," + RolesList.HeadOfTraining)]
    [HttpDelete("deletesheet")]
    public async Task<IActionResult> DeleteSheet(int sheetId) {
        if (!await IsAuthorized(await GetCommunityId(sheetId))) return Forbid();
        var result = await _sheetRepository.deleteSheet(sheetId);
        if (!result.Succeeded) {
            return BadRequest(result.Errors);
        }
        return Ok(result);
    }
    [HttpGet("getsheet")]
    public async Task<IActionResult> GetSheet(int sheetId) {
        var result = await _sheetRepository.GetTheSheet(sheetId);
        if (!result.Succeeded) {
            return NotFound(result.Errors);
        }
        return Ok(result);
    }
    [HttpGet("getsheetsbycommunity")]
    public async Task<IActionResult> GetSheetsByCommunty(int communityId) {
        var result = await _sheetRepository.GetSheetsByCommunity(communityId);
        if (!result.Succeeded) {
            return BadRequest(result.Errors);
        }
        return Ok(result);
    }
    [HttpGet("getsheetsbyweek")]
    public async Task<IActionResult> GetSheetsByWeek(int weekId) {
        var result = await _sheetRepository.GetSheetsByWeek(weekId);
        if (!result.Succeeded) {
            return BadRequest(result.Errors);
        }
        return Ok(result);
    }
}