using ICPC_WebSite_Backend.Data;
using ICPC_WebSite_Backend.Data.Models;
using Microsoft.EntityFrameworkCore;
using UtilityLibrary.ModelsDTO;
using UtilityLibrary.Response;
using UtilityLibrary.Utility;

namespace ICPC_WebSite_Backend.Repository;

public class SheetRepository : ISheetRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    public SheetRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<Response<Sheet>> AddSheet(SheetDto sheetDto)
    {
        var community = await _applicationDbContext.Communities.FindAsync(sheetDto.CommunityId);
        if (community == null)
            return ResponseFactory.Fail<Sheet>(ErrorsList.CommunityNotFound);
        var sheet = new Sheet()
                    {
                        Name        = sheetDto.Name,
                        CreatedAt   = DateTime.Now,
                        Url         = sheetDto.Url,
                        ContestId = sheetDto.ContestId,
                        CommunityId = sheetDto.CommunityId
                    };
        await _applicationDbContext.Sheets.AddAsync(sheet);
        await _applicationDbContext.SaveChangesAsync();
        return ResponseFactory.Ok(sheet);
    }

    public async Task<Response> UpdateSheet(int sheetId, SheetDto sheetDto)
    {
        var sheet = await _applicationDbContext.Sheets.FindAsync(sheetId);

        if (sheet == null) return ResponseFactory.Fail(ErrorsList.SheetNotFound);
        sheet.Url       = sheetDto.Url;
        sheet.Name      = sheetDto.Name;
        sheet.ContestId = sheetDto.ContestId;
        sheet.UpdatedAt = DateTime.Now;
        await _applicationDbContext.SaveChangesAsync();
        return ResponseFactory.Ok(sheet);
    }

    public async Task<Response<Sheet>> GetTheSheet(int sheetId)
    {
        var sheet = await _applicationDbContext.Sheets.FindAsync(sheetId);
        return sheet == null ? ResponseFactory.Fail<Sheet>(ErrorsList.SheetNotFound) : ResponseFactory.Ok(sheet);
    }

    public async Task<Response<IEnumerable<Sheet>>> GetSheetsByCommunity(int communityId)
    {
        var sheets = await _applicationDbContext.Sheets.Where(s=>s.CommunityId==communityId).ToListAsync();
        return ResponseFactory.Ok<IEnumerable<Sheet>>(sheets);
    }

    public async Task<Response<IEnumerable<Sheet>>> GetSheetsByWeek(int weekId)
    {
        var sheets = await _applicationDbContext.Sheets
                                                .Where(a => a.WeekSheets
                                                             .Any(c => c.WeekId == weekId))
                                                .ToListAsync();
        return ResponseFactory.Ok<IEnumerable<Sheet>>(sheets);
    }
    
    public async Task<Response> deleteSheet(int sheetId)
    {
        var sheet = await _applicationDbContext.Sheets.FindAsync(sheetId);
        if (sheet == null) return ResponseFactory.Fail(ErrorsList.SheetNotFound);
        _applicationDbContext.Sheets.Remove(sheet);
        await _applicationDbContext.SaveChangesAsync();

        return ResponseFactory.Ok();
    }
}