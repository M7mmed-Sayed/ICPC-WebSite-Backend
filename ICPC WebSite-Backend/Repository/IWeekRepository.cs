using ICPC_WebSite_Backend.Data.Models;
using ICPC_WebSite_Backend.Data.Models.DTO;
using ICPC_WebSite_Backend.Data.Models.ReturnObjects;
using ICPC_WebSite_Backend.Data.Response;

namespace ICPC_WebSite_Backend.Repository
{
    public interface IWeekRepository
    {
        Task<Response> AddWeek(WeekDto weekDto);
        Task<Response> UpdateWeek(int weekId, WeekDto weekDto);
        Task<Response<IEnumerable<Week>>> GetAllTemplateWeeks();
        Task<Response<IEnumerable<Week>>> GetWeeksByCommunity(int communityId);
        Task<Response<IEnumerable<Week>>> GetWeeksByTraining(int trainingId);
        Task<Response<Week>> GetTheWeek(int weekId);
        Task<Response> CreateTemplateWeek(int weekId);
        Task<Response> deleteWeek(int weekId);
        Task<Response> LinkSheet(int weekId, int sheetId);
      
        Task<Response> UnLinkSheet(int weekId, int sheetId);
    }
}
