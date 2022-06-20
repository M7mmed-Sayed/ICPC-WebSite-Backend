using ICPC_WebSite_Backend.Data.Models;
using ICPC_WebSite_Backend.Data.Models.DTO;
using ICPC_WebSite_Backend.Data.Models.ReturnObjects;
using ICPC_WebSite_Backend.Data.Response;

namespace ICPC_WebSite_Backend.Repository
{
    public interface IWeekRepository
    {
        Task<Response> AddWeek(WeekDTO weekDTO);
        Task<Response> UpdateWeek(int weekId, WeekDTO weekDTO);
        Task<Response<IEnumerable<Week>>> GetAllTemplateWeeks();
        Task<Response<IEnumerable<Week>>> GetAllWeeks();
        Task<Response<Week>> GetTheWeek(int weekId);
        Task<Response> createTemplateWeek(int weekId);
    }
}
