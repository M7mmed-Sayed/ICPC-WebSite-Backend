using ICPC_WebSite_Backend.Data;
using ICPC_WebSite_Backend.Data.Models;
using Microsoft.EntityFrameworkCore;
using UtilityLibrary.ModelsDTO;
using UtilityLibrary.Response;
using UtilityLibrary.Utility;

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
        var community = await _applicationDbContext.Communities.FindAsync(weekDto.CommunityId);
        if (community == null)
            return ResponseFactory.Fail(ErrorsList.CommunityNotFound);
        var week = new Week
                   {
                       Name        = weekDto.Name,
                       Description = weekDto.Description,
                       CreatedAt   = DateTime.Now,
                       CommunityId = weekDto.CommunityId
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
        week.Name        = weekDto.Name;
        week.UpdatedAt   = DateTime.Now;
        await _applicationDbContext.SaveChangesAsync();
        return ResponseFactory.Ok();
    }

    
    public async Task<Response<IEnumerable<Week>>> GetWeeksByCommunity(int communityId)
    {
        var weeks = await _applicationDbContext.Weeks.Where(w => w.Id == communityId).ToListAsync();
        return ResponseFactory.Ok<IEnumerable<Week>>(weeks);
    }

    public async Task<Response<IEnumerable<Week>>> GetWeeksByTraining(int trainingId)
    {
        var weeks = await _applicationDbContext.Weeks.Distinct()
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
        if (errors.Count != 0)
            return ResponseFactory.Fail(errors);
        var weekSheet =
            _applicationDbContext.WeeksSheets.FirstOrDefault(w => w.WeekId == weekId && w.SheetId == sheetId);
        if (weekSheet != null)
            return ResponseFactory.Fail(ErrorsList.DublicateSheetAtWeek);
        weekSheet = new WeekSheet {WeekId = weekId, SheetId = sheetId};
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