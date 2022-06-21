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

    public async Task<Response> AddWeek(WeekDto weekDto)
    {
        var week = new Week
        {
            Name = weekDto.Name,
            Description = weekDto.Description,
            IsTemplate = weekDto.IsTemplate,
            CreatedAt = DateTime.Now,
            Training_Id = weekDTO.TrainingId
        };
        await _applicationDbContext.Weeks.AddAsync(week);
        await _applicationDbContext.SaveChangesAsync();
        return ResponseFactory.Ok();
    }

    public async Task<Response> UpdateWeek(int weekId, WeekDto weekDto)
    {
        var week = await _applicationDbContext.Weeks.FindAsync(weekId);

        if (week == null) return ResponseFactory.Fail(ErrorsList.WeekNotFound);
        week.Description = weekDto.Description;
        week.IsTemplate = weekDto.IsTemplate;
        week.Name = weekDto.Name;
        week.UpdatedAt = DateTime.Now;
        await _applicationDbContext.SaveChangesAsync();
        return ResponseFactory.Ok();
    }

    public async Task<Response<IEnumerable<Week>>> GetAllTemplateWeeks()
    {
        var weeks = await _applicationDbContext.Weeks.Where(w => w.IsTemplate == true).ToListAsync();
        return ResponseFactory.Ok<IEnumerable<Week>>(weeks);
    }

    public async Task<Response<IEnumerable<Week>>> GetAllWeeks()
    {
        var weeks = await _applicationDbContext.Weeks.ToListAsync();
        return ResponseFactory.Ok<IEnumerable<Week>>(weeks);
    }

    public async Task<Response<Week>> GetTheWeek(int weekId)
    {
        var week = await _applicationDbContext.Weeks.FindAsync(weekId);

        return week == null ? ResponseFactory.Fail<Week>(ErrorsList.WeekNotFound) : ResponseFactory.Ok(week);
    }

    public async Task<Response> CreateTemplateWeek(int weekId)
    {
        var week = await _applicationDbContext.Weeks.FindAsync(weekId);

        if (week == null) return ResponseFactory.Fail(ErrorsList.WeekNotFound);

        var newWeek = new Week
        {
            CreatedAt = DateTime.Now,
            Description = week.Description,
            Name = week.Name,
            IsTemplate = false
        };
        await _applicationDbContext.Weeks.AddAsync(newWeek);
        var weekMaterials = _applicationDbContext.Materials.Where(m => m.WeekId == weekId).ToList();

        foreach (var material in weekMaterials)
            await _applicationDbContext.Materials.AddAsync(new Material
            {
                CreatedAt = DateTime.Now,
                Description = material.Description,
                WeekId = newWeek.Id,
                Url = material.Url
            });
        await _applicationDbContext.SaveChangesAsync();

        return ResponseFactory.Ok();
    }
    public async Task<Response> deleteWeek(int weekId) {
        var ret = new Response();
        var week = await _applicationDbContext.weeks.FindAsync(weekId);
        if (week != null) {
            _applicationDbContext.weeks.Remove(week);
            await _applicationDbContext.SaveChangesAsync();
        }
        else {
            ret.Succeeded = false;
            ret.Errors.Add(ErrorsList.WeekNotFound);
        }
        return ret;

    }
}