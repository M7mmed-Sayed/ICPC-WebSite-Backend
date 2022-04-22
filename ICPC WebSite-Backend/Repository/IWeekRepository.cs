using ICPC_WebSite_Backend.Models;
using ICPC_WebSite_Backend.Models.DTO;
using ICPC_WebSite_Backend.Data.ReturnObjects.Models;
using ICPC_WebSite_Backend.Data.Models.DTO;

namespace ICPC_WebSite_Backend.Repository
{
    public interface IWeekRepository
    {
        Task<Response> AddWeek(WeekDTO weekDTO);
        Task<Response> UpdateWeek(int weekId, WeekDTO weekDTO);
        Task<Response> GetAllTemplateWeeks();
        Task<Response> GetAllWeeks();
        Task<Response> GetTheWeek(int weekId);
        Task<Response> createTemplateWeek(int weekId);
    }
}
