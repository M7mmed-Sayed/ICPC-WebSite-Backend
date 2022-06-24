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
        var sheet = new Sheet()
        {
            Name = sheetDto.Name,
            CreatedAt = DateTime.Now,
            Url=sheetDto.Url,
            CommunityId = sheetDto.CommunityId
        };
        await _applicationDbContext.Sheets.AddAsync(sheet);
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

   
    public async Task<Response> deleteSheet(int sheetId)
    {
        var sheet = await _applicationDbContext.Sheets.FindAsync(sheetId);
        if (sheet == null) return ResponseFactory.Fail(ErrorsList.SheetNotFound);

        _applicationDbContext.Sheets.Remove(sheet);
        await _applicationDbContext.SaveChangesAsync();

        return ResponseFactory.Ok();
    }
    
}