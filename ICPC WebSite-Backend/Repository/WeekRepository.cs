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
        try
        {
            var community = await _applicationDbContext.Communities.FindAsync(weekDto.CommunityId);
            if (community == null)
                return ResponseFactory.Fail(ErrorsList.CommunityNotFound);
            var week = new Week
            {
                Name = weekDto.Name,
                Description = weekDto.Description,
                CreatedAt = DateTime.Now,
                CommunityId = weekDto.CommunityId
            };
            await _applicationDbContext.Weeks.AddAsync(week);
            await _applicationDbContext.SaveChangesAsync();
            return ResponseFactory.Ok();
        }
        catch (Exception ex)
        {
            return ResponseFactory.FailFromException(ex);
        }
    }

    public async Task<Response> UpdateWeek(int weekId, WeekDto weekDto)
    {
        try
        {
            var week = await _applicationDbContext.Weeks.FindAsync(weekId);

            if (week == null) return ResponseFactory.Fail(ErrorsList.WeekNotFound);
            week.Description = weekDto.Description;
            week.Name = weekDto.Name;
            week.UpdatedAt = DateTime.Now;
            await _applicationDbContext.SaveChangesAsync();
            return ResponseFactory.Ok();
        }
        catch (Exception ex)
        {
            return ResponseFactory.FailFromException(ex);
        }
    }


    public async Task<Response<IEnumerable<WeekResponse>>> GetWeeksByCommunity(int communityId)
    {
        try
        {
            var community = await _applicationDbContext.Communities.Where(community=>community.Id==communityId).Include(x=>x.Weeks).FirstOrDefaultAsync();
            if (community == null)
                return ResponseFactory.Fail<IEnumerable<WeekResponse>>(ErrorsList.CommunityNotFound);
            var data = community.Weeks.Select(week =>new WeekResponse()
            {
                Id=week.Id,
                Name = week.Name,
                Description = week.Description
            } ).ToList();
            return ResponseFactory.Ok<IEnumerable<WeekResponse>>(data);

        }
        catch (Exception ex)
        {
            return ResponseFactory.FailFromException<IEnumerable<WeekResponse>>(ex);
        }
    }

    public async Task<Response<IEnumerable<WeekResponse>>> GetWeeksByTraining(int trainingId)
    {
        try
        {
            var weeks = await _applicationDbContext.Weeks.Distinct()
                .Where(a => a.WeekTraining
                    .Any(c => c.TrainingId == trainingId)).Select(week =>new WeekResponse()
                    {
                        Id=week.Id,
                        Name = week.Name,
                        Description = week.Description
                    } )
                .ToListAsync();
            
            return ResponseFactory.Ok<IEnumerable<WeekResponse>>(weeks);
        }
        catch (Exception ex)
        {
            return ResponseFactory.FailFromException<IEnumerable<WeekResponse>>(ex);
        }
    }

    public async Task<Response<Week>> GetTheWeek(int weekId)
    {
        try
        {
            var week = await _applicationDbContext.Weeks.FindAsync(weekId);

            return week == null ? ResponseFactory.Fail<Week>(ErrorsList.WeekNotFound) : ResponseFactory.Ok(week);
        }
        catch (Exception ex)
        {
            return ResponseFactory.FailFromException<Week>(ex);
        }
    }


    public async Task<Response> deleteWeek(int weekId)
    {
        try
        {
            var week = await _applicationDbContext.Weeks.FindAsync(weekId);
            if (week == null) return ResponseFactory.Fail(ErrorsList.WeekNotFound);
            _applicationDbContext.Weeks.Remove(week);
            await _applicationDbContext.SaveChangesAsync();

            return ResponseFactory.Ok();
        }
        catch (Exception ex)
        {
            return ResponseFactory.FailFromException(ex);
        }
    }

    public async Task<Response> LinkSheet(int weekId, int sheetId)
    {
        try
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
            weekSheet = new WeekSheet { WeekId = weekId, SheetId = sheetId };
            await _applicationDbContext.WeeksSheets.AddAsync(weekSheet);
            await _applicationDbContext.SaveChangesAsync();
            return ResponseFactory.Ok();
        }
        catch (Exception ex)
        {
            return ResponseFactory.FailFromException(ex);
        }
    }


    public async Task<Response> UnLinkSheet(int weekId, int sheetId)
    {
        try
        {
            var weekSheet =
                _applicationDbContext.WeeksSheets.FirstOrDefault(w => w.WeekId == weekId && w.SheetId == sheetId);
            if (weekSheet == null)
                return ResponseFactory.Fail(ErrorsList.SheetNotAtTheWeek);
            _applicationDbContext.WeeksSheets.Remove(weekSheet);
            await _applicationDbContext.SaveChangesAsync();
            return ResponseFactory.Ok();
        }
        catch (Exception ex)
        {
            return ResponseFactory.FailFromException(ex);
        }
    }
}