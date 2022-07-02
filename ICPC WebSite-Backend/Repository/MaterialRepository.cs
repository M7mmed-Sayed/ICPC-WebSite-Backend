using ICPC_WebSite_Backend.Data;
using ICPC_WebSite_Backend.Data.Models;
using Microsoft.EntityFrameworkCore;
using UtilityLibrary.ModelsDTO;
using UtilityLibrary.Response;
using UtilityLibrary.Utility;

namespace ICPC_WebSite_Backend.Repository;

public class MaterialRepository : IMaterialRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    public MaterialRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<Response> AddMaterial(MaterialDto materialDto)
    {
        var week = await _applicationDbContext.Weeks.FindAsync(materialDto.WeekId);

        if (week == null) return ResponseFactory.Fail(ErrorsList.WeekNotFound);

        await _applicationDbContext.SaveChangesAsync();
        var material = new Material
        {
            CreatedAt = DateTime.Now,
            Url = materialDto.Url,
            Description = materialDto.Description,
            WeekId = materialDto.WeekId
        };
        await _applicationDbContext.Materials.AddAsync(material);
        await _applicationDbContext.SaveChangesAsync();
        return ResponseFactory.Ok();
    }

    public async Task<Response> DeleteMaterial(int materialId)
    {
        var material = await _applicationDbContext.Materials.FindAsync(materialId);

        if (material == null)
            return ResponseFactory.Fail(ErrorsList.MaterailNotFound);
        _applicationDbContext.Remove(material);
        await _applicationDbContext.SaveChangesAsync();
        return ResponseFactory.Ok();
    }

    public async Task<Response<IEnumerable<Material>>> GetWeekMaterials(int weekId)
    {
        var materials = await _applicationDbContext.Materials.Where(m => m.WeekId == weekId).ToListAsync();
        return ResponseFactory.Ok<IEnumerable<Material>>(materials);
    }

    public async Task<Response> UpdateMaterial(int materialId, MaterialDto materialDto)
    {
        var material = await _applicationDbContext.Materials.FindAsync(materialId);

        if (material == null) return ResponseFactory.Fail(ErrorsList.MaterailNotFound);
        
        material.Description = materialDto.Description;
        material.Url = materialDto.Url;
        await _applicationDbContext.SaveChangesAsync();
        return ResponseFactory.Ok();
    }
}