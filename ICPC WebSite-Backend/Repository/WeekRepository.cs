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
        var training = await _applicationDbContext.Trainings.FindAsync(weekDto.TrainingId);
        if (training == null)
            return ResponseFactory.Fail(ErrorsList.TrainingNotFound);
        var week = new Week
        {
            Name = weekDto.Name,
            Description = weekDto.Description,
            IsTemplate = weekDto.IsTemplate,
            CreatedAt = DateTime.Now,
          //  CommunityId = weekDto.CommunityId
        };
        await _applicationDbContext.Weeks.AddAsync(week);
        await _applicationDbContext.SaveChangesAsync();
        var weekTraining = new WeekTraining()
        {
            TrainingId = weekDto.TrainingId,
            WeekId = week.Id
        };
        await _applicationDbContext.WeeksTrainings.AddAsync(weekTraining);
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

    public async Task<Response<IEnumerable<Week>>> GetWeeksByCommunity(int communityId)
    {
        var weeks = await _applicationDbContext.Weeks.Where(w => w.Id == communityId).ToListAsync();
        return ResponseFactory.Ok<IEnumerable<Week>>(weeks);
    }
    public async Task<Response<IEnumerable<Week>>> GetWeeksByTraining(int trainingId)
    {
        var weeks = await _applicationDbContext.Weeks
            .Where(a => a.WeekTraining
                .Any(c => c.TrainingId == trainingId))
            .ToListAsync();
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

    public async Task<Response> deleteWeek(int weekId)
    {
        var week = await _applicationDbContext.Weeks.FindAsync(weekId);
        if (week == null) return ResponseFactory.Fail(ErrorsList.WeekNotFound);
        _applicationDbContext.Weeks.Remove(week);
        await _applicationDbContext.SaveChangesAsync();

        return ResponseFactory.Ok();
    }

    public async Task<Response> LinkSheet(int weekId, int sheetId)
    {
        var week = await _applicationDbContext.Weeks.FindAsync(weekId);
        var errors = new List<Error>();
        if (week == null)
            errors.Add(ErrorsList.WeekNotFound);

        var sheet = await _applicationDbContext.Sheets.FindAsync(weekId);
        if (sheet == null)
            errors.Add(ErrorsList.SheetNotFound);
        if (errors.Count() != 0)
            return ResponseFactory.Fail(errors);
        var weekSheet =
            _applicationDbContext.WeeksSheets.FirstOrDefault(w => w.WeekId == weekId && w.SheetId == sheetId);
        if (weekSheet != null)
            return ResponseFactory.Fail(ErrorsList.DublicateSheetAtWeek);
        weekSheet = new WeekSheet { WeekId = weekId, SheetId = sheetId };
        await _applicationDbContext.WeeksSheets.AddAsync(weekSheet);
        await _applicationDbContext.SaveChangesAsync();
        return ResponseFactory.Ok();
    }

  

    public async Task<Response> UnLinkSheet(int weekId, int sheetId)
    {
        var weekSheet =
            _applicationDbContext.WeeksSheets.FirstOrDefault(w => w.WeekId == weekId && w.SheetId == sheetId);
        if (weekSheet == null)
            return ResponseFactory.Fail(ErrorsList.SheetNotAtTheWeek);
        _applicationDbContext.WeeksSheets.Remove(weekSheet);
        await _applicationDbContext.SaveChangesAsync();
        return ResponseFactory.Ok();
    }
}