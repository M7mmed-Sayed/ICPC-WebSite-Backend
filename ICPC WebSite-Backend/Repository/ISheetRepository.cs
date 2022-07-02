using ICPC_WebSite_Backend.Data.Models;
using UtilityLibrary.ModelsDTO;
using UtilityLibrary.Response;

namespace ICPC_WebSite_Backend.Repository;

public interface ISheetRepository
{
    Task<Response> AddSheet(SheetDto sheetDto);
    Task<Response> UpdateSheet(int sheetId, SheetDto SheetDto);
    Task<Response<Sheet>> GetTheSheet(int sheetId);
    Task<Response> deleteSheet(int sheetId);
    Task<Response<IEnumerable<Sheet>>> GetSheetsByCommunity(int communityId);
    Task<Response<IEnumerable<Sheet>>> GetSheetsByWeek(int weekId);
}