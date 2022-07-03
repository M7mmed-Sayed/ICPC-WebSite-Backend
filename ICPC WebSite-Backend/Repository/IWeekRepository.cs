using ICPC_WebSite_Backend.Data.Models;
using UtilityLibrary.ModelsDTO;
using UtilityLibrary.Response;

namespace ICPC_WebSite_Backend.Repository
{
    public interface IWeekRepository
    {
        Task<Response<Week>> AddWeek(WeekDto weekDto);
        Task<Response> UpdateWeek(int weekId, WeekDto weekDto);
        Task<Response<IEnumerable<WeekResponse>>> GetWeeksByCommunity(int communityId);
        Task<Response<IEnumerable<WeekResponse>>> GetWeeksByTraining(int trainingId);
        Task<Response<Week>> GetTheWeek(int weekId);
        Task<Response> deleteWeek(int weekId);
        Task<Response> LinkSheet(int weekId, int sheetId);
      
        Task<Response> UnLinkSheet(int weekId, int sheetId);
    }
}
