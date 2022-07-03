using ICPC_WebSite_Backend.Data.Models;
using UtilityLibrary.ModelsDTO;
using UtilityLibrary.Response;

namespace ICPC_WebSite_Backend.Repository;

public interface ISheetRepository
{
    /// <summary>
    /// add a sheet to a community
    /// </summary>
    /// <param name="sheetDto">the data of the sheet</param>
    /// <returns>returns failed or succeeded</returns>
    Task<Response<Sheet>> AddSheet(SheetDto sheetDto);
    /// <summary>
    /// update a sheet with new data
    /// </summary>
    /// <param name="sheetId">the id of the sheet</param>
    /// <param name="SheetDto">the new data of the sheet</param>
    /// <returns>returns failed or succeeded</returns>
    Task<Response> UpdateSheet(int sheetId, SheetDto SheetDto);
    /// <summary>
    /// get a specific sheet
    /// </summary>
    /// <param name="sheetId">the id of the sheet</param>
    /// <returns>the data of the sheet</returns>
    Task<Response<Sheet>> GetTheSheet(int sheetId);
    /// <summary>
    /// delete a specific sheet
    /// </summary>
    /// <param name="sheetId">the id of the sheet</param>
    /// <returns>returns failed or succeeded</returns>
    Task<Response> deleteSheet(int sheetId);
    /// <summary>
    /// get all sheets in a specific community
    /// </summary>
    /// <param name="communityId">the id of the community</param>
    /// <returns>list of all sheets in the community</returns>
    Task<Response<IEnumerable<Sheet>>> GetSheetsByCommunity(int communityId);
    /// <summary>
    /// get all sheets in a specific week
    /// </summary>
    /// <param name="weekId">the id of the week</param>
    /// <returns>list of sheets data</returns>
    Task<Response<IEnumerable<Sheet>>> GetSheetsByWeek(int weekId);
}