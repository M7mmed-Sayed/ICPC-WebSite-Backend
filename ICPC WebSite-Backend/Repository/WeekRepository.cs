using ICPC_WebSite_Backend.Data;
using ICPC_WebSite_Backend.Data.Models;
using ICPC_WebSite_Backend.Data.Models.DTO;
using ICPC_WebSite_Backend.Data.Response;
using ICPC_WebSite_Backend.Utility;
using Microsoft.EntityFrameworkCore;

namespace ICPC_WebSite_Backend.Repository;

public class WeekRepository : IWeekRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    public WeekRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<Response> AddWeek(WeekDTO weekDTO)
    {
        var week = new Week
        {
            Name = weekDTO.Name,
            Description = weekDTO.Description,
            IsTemplate = weekDTO.IsTemplate,
            Created_at = DateTime.Now
        };
        await _applicationDbContext.weeks.AddAsync(week);
        await _applicationDbContext.SaveChangesAsync();
        return ResponseFactory.Ok();
    }

    public async Task<Response> UpdateWeek(int weekId, WeekDTO weekDTO)
    {
        var week = await _applicationDbContext.weeks.FindAsync(weekId);

        if (week == null) return ResponseFactory.Fail(ErrorsList.WeekNotFound);
        week.Description = weekDTO.Description;
        week.IsTemplate = weekDTO.IsTemplate;
        week.Name = weekDTO.Name;
        week.Updated_at = DateTime.Now;
        await _applicationDbContext.SaveChangesAsync();
        return ResponseFactory.Ok();
    }

    public async Task<Response<IEnumerable<Week>>> GetAllTemplateWeeks()
    {
        var weeks = await _applicationDbContext.weeks.Where(W => W.IsTemplate == true).ToListAsync();
        return ResponseFactory.Ok<IEnumerable<Week>>(weeks);
    }

    public async Task<Response<IEnumerable<Week>>> GetAllWeeks()
    {
        var weeks = await _applicationDbContext.weeks.ToListAsync();
        return ResponseFactory.Ok<IEnumerable<Week>>(weeks);
    }

    public async Task<Response<Week>> GetTheWeek(int weekId)
    {
        var week = await _applicationDbContext.weeks.FindAsync(weekId);

        return week == null ? ResponseFactory.Fail<Week>(ErrorsList.WeekNotFound) : ResponseFactory.Ok(week);
    }

    public async Task<Response> createTemplateWeek(int weekId)
    {
        var week = await _applicationDbContext.weeks.FindAsync(weekId);

        if (week == null) return ResponseFactory.Fail(ErrorsList.WeekNotFound);

        var newWeek = new Week
        {
            Created_at = DateTime.Now,
            Description = week.Description,
            Name = week.Name,
            IsTemplate = false
        };
        await _applicationDbContext.weeks.AddAsync(newWeek);
        var weekMaterials = _applicationDbContext.Materials.Where(m => m.weekId == weekId).ToList();

        foreach (var material in weekMaterials)
            await _applicationDbContext.Materials.AddAsync(new Material
            {
                Created_at = DateTime.Now,
                Description = material.Description,
                weekId = newWeek.Id,
                URL = material.URL
            });
        await _applicationDbContext.SaveChangesAsync();

        return ResponseFactory.Ok();
    }
}