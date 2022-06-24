using ICPC_WebSite_Backend.Data.Models.DTO;
using ICPC_WebSite_Backend.Repository;
using Microsoft.AspNetCore.Mvc;

namespace ICPC_WebSite_Backend.Controllers;

public class SheetController : Controller
{
    private readonly ISheetRepository _sheetRepository;
    public SheetController(ISheetRepository sheetRepository) {
        _sheetRepository = sheetRepository;
    }
    [HttpPost("addsheet")]
    public async Task<IActionResult> addtraining(SheetDto sheetDto) {
        
        var result = await _sheetRepository.AddSheet(sheetDto);
        if (!result.Succeeded) {
            return Unauthorized(result.Errors);
        }
        return Ok(result);
    }
    [HttpPut("updatesheet")]
    public async Task<IActionResult> Updatesheet(int sheetId,SheetDto sheetDto) {
        
        var result = await _sheetRepository.UpdateSheet(sheetId,sheetDto);
        if (!result.Succeeded) {
            return Unauthorized(result.Errors);
        }
        return Ok(result);
    }
    [HttpDelete("deletesheet")]
    public async Task<IActionResult> DeleteSheet(int sheetId) {
        var result = await _sheetRepository.deleteSheet(sheetId);
        if (!result.Succeeded) {
            return NotFound(result.Errors);
        }
        return Ok(result);
    }
    [HttpGet("getsheet")]
    public async Task<IActionResult> GetSheet(int sheetId) {
        var result = await _sheetRepository.GetTheSheet(sheetId);
        if (!result.Succeeded) {
            return Unauthorized(result.Errors);
        }
        return Ok(result);
    }
    [HttpGet("getsheetsbycommunity")]
    public async Task<IActionResult> GetSheetsByCommunty(int communityId) {
        var result = await _sheetRepository.GetSheetsByCommunity(communityId);
        if (!result.Succeeded) {
            return Unauthorized(result.Errors);
        }
        return Ok(result);
    }
    [HttpGet("getsheetsbyweek")]
    public async Task<IActionResult> GetSheetsByWeek(int weekId) {
        var result = await _sheetRepository.GetSheetsByWeek(weekId);
        if (!result.Succeeded) {
            return Unauthorized(result.Errors);
        }
        return Ok(result);
    }
}