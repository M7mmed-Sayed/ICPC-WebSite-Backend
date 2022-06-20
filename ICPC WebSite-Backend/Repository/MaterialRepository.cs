using ICPC_WebSite_Backend.Data;
using ICPC_WebSite_Backend.Data.Models;
using ICPC_WebSite_Backend.Data.Models.DTO;
using ICPC_WebSite_Backend.Data.Response;
using ICPC_WebSite_Backend.Utility;
using Microsoft.EntityFrameworkCore;

namespace ICPC_WebSite_Backend.Repository;

public class MaterialRepository : IMaterialRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    public MaterialRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<Response> addMaterial(MaterialDTO MaterialDTO)
    {
        var week = await _applicationDbContext.weeks.FindAsync(MaterialDTO.weekId);

        if (week == null) return ResponseFactory.Fail(ErrorsList.WeekNotFound);

        await _applicationDbContext.SaveChangesAsync();
        var Material = new Material
        {
            Created_at = DateTime.Now,
            URL = MaterialDTO.URL,
            Description = MaterialDTO.Description,
            weekId = MaterialDTO.weekId
        };
        await _applicationDbContext.Materials.AddAsync(Material);
        await _applicationDbContext.SaveChangesAsync();
        return ResponseFactory.Ok();
    }

    public async Task<Response> deleteMaterial(int materialId)
    {
        var material = await _applicationDbContext.Materials.FindAsync(materialId);

        if (material == null)
            return ResponseFactory.Fail(ErrorsList.MaterailNotFound);
        _applicationDbContext.Remove(material);
        await _applicationDbContext.SaveChangesAsync();
        return ResponseFactory.Ok();
    }

    public async Task<Response<IEnumerable<Material>>> getWeekMaterials(int weekId)
    {
        var _materials = await _applicationDbContext.Materials.Where(m => m.weekId == weekId).ToListAsync();
        return ResponseFactory.Ok<IEnumerable<Material>>(_materials);
    }

    public async Task<Response> updateMaterial(int materialId, MaterialDTO MaterialDTO)
    {
        var material = await _applicationDbContext.Materials.FindAsync(materialId);

        if (material == null) return ResponseFactory.Fail(ErrorsList.MaterailNotFound);
        
        material.Description = MaterialDTO.Description;
        material.URL = MaterialDTO.URL;
        await _applicationDbContext.SaveChangesAsync();
        return ResponseFactory.Ok();
    }
}