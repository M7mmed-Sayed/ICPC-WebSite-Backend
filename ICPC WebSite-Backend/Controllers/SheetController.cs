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

    /// <summary>
    /// add a sheet to a community
    /// </summary>
    /// <param name="sheetDto">the data of the sheet</param>
    /// <returns>returns failed or succeeded</returns>
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
    /// <summary>
    /// update a sheet with new data
    /// </summary>
    /// <param name="sheetId">the id of the sheet</param>
    /// <param name="SheetDto">the new data of the sheet</param>
    /// <returns>returns failed or succeeded</returns>
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
    /// <summary>
    /// delete a specific sheet
    /// </summary>
    /// <param name="sheetId">the id of the sheet</param>
    /// <returns>returns failed or succeeded</returns>
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
    /// <summary>
    /// get a specific sheet
    /// </summary>
    /// <param name="sheetId">the id of the sheet</param>
    /// <returns>the data of the sheet</returns>
    [HttpGet("getsheet")]
    public async Task<IActionResult> GetSheet(int sheetId) {
        var result = await _sheetRepository.GetTheSheet(sheetId);
        if (!result.Succeeded) {
            return NotFound(result.Errors);
        }
        return Ok(result);
    }
    /// <summary>
    /// get all sheets in a specific community
    /// </summary>
    /// <param name="communityId">the id of the community</param>
    /// <returns>list of all sheets in the community</returns>
    [HttpGet("getsheetsbycommunity")]
    public async Task<IActionResult> GetSheetsByCommunty(int communityId) {
        var result = await _sheetRepository.GetSheetsByCommunity(communityId);
        if (!result.Succeeded) {
            return BadRequest(result.Errors);
        }
        return Ok(result);
    }
    /// <summary>
    /// get all sheets in a specific week
    /// </summary>
    /// <param name="weekId">the id of the week</param>
    /// <returns>list of sheets data</returns>
    [HttpGet("getsheetsbyweek")]
    public async Task<IActionResult> GetSheetsByWeek(int weekId) {
        var result = await _sheetRepository.GetSheetsByWeek(weekId);
        if (!result.Succeeded) {
            return BadRequest(result.Errors);
        }
        return Ok(result);
    }
}