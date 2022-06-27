﻿using ICPC_WebSite_Backend.Data;
using ICPC_WebSite_Backend.Data.Models;
using ICPC_WebSite_Backend.Data.Models.DTO;
using ICPC_WebSite_Backend.Data.Response;
using ICPC_WebSite_Backend.Utility;
using Microsoft.EntityFrameworkCore;

namespace ICPC_WebSite_Backend.Repository;

public class SheetRepository:ISheetRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    public SheetRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<Response> AddSheet(SheetDto sheetDto)
    {
        var week = await _applicationDbContext.Weeks.FindAsync(sheetDto.WeekId);
        if (week == null)
            return ResponseFactory.Fail(ErrorsList.WeekNotFound);
        var sheet = new Sheet()
        {
            Name = sheetDto.Name,
            CreatedAt = DateTime.Now,
            Url=sheetDto.Url
        };
        await _applicationDbContext.Sheets.AddAsync(sheet);
        await _applicationDbContext.SaveChangesAsync();
        var weekSheet = new WeekSheet()
        {
            SheetId = sheet.Id,
            WeekId = week.Id
        };
        await _applicationDbContext.WeeksSheets.AddAsync(weekSheet);
        await _applicationDbContext.SaveChangesAsync();
        return ResponseFactory.Ok();
    }

    public async Task<Response> UpdateSheet(int sheetId, SheetDto SheetDto)
    {
        var sheet = await _applicationDbContext.Sheets.FindAsync(sheetId);

        if (sheet == null) return ResponseFactory.Fail(ErrorsList.SheetNotFound);
        sheet.Url =sheet.Url;
        sheet.Name = SheetDto.Name;
        sheet.UpdatedAt = DateTime.Now;
        await _applicationDbContext.SaveChangesAsync();
        return ResponseFactory.Ok();
    }
    
    public async Task<Response<Sheet>> GetTheSheet(int sheetId)
    {
        var sheet = await _applicationDbContext.Sheets.FindAsync(sheetId);
        return sheet==null? ResponseFactory.Fail<Sheet>(ErrorsList.SheetNotFound) : ResponseFactory.Ok(sheet);
    }
    public async Task<Response<IEnumerable<Sheet>>> GetSheetsByCommunity(int communityId)
    {
        var sheets = await _applicationDbContext.Sheets.Where(sh => sh.CommunityId == communityId).ToListAsync();
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