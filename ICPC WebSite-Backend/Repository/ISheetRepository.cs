using ICPC_WebSite_Backend.Data.Models;
using ICPC_WebSite_Backend.Data.Models.DTO;
using ICPC_WebSite_Backend.Data.Response;

namespace ICPC_WebSite_Backend.Repository;

public interface ISheetRepository
{
    Task<Response> AddSheet(SheetDto sheetDto);
    Task<Response> UpdateSheet(int sheetId, SheetDto SheetDto);
    Task<Response<Sheet>> GetTheSheet(int sheetId);
    Task<Response> deleteSheet(int sheetId);
}