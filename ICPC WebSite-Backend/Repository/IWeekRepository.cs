using ICPC_WebSite_Backend.Data.Models;
using UtilityLibrary.ModelsDTO;
using UtilityLibrary.Response;

namespace ICPC_WebSite_Backend.Repository
{
    public interface IWeekRepository
    {
        /// <summary>
        /// add a week to a community
        /// </summary>
        /// <param name="weekDto">contains week important data like week id and community id</param>
        /// <returns>failed or succeeded</returns>
        Task<Response<Week>> AddWeek(WeekDto weekDto);
        /// <summary>
        /// update a week with new data
        /// </summary>
        /// <param name="weekId">id the week</param>
        /// <param name="weekDto">the new data</param>
        /// <returns>failed or succeeded</returns>
        Task<Response> UpdateWeek(int weekId, WeekDto weekDto);
        /// <summary>
        /// get all week inside a community
        /// </summary>
        /// <param name="communityId">id of the community</param>
        /// <returns>if succeeded return list of week otherwise returns an error</returns>
        Task<Response<IEnumerable<WeekResponse>>> GetWeeksByCommunity(int communityId);
        /// <summary>
        /// get all week inside a training
        /// </summary>
        /// <param name="trainingId">id of the training</param>
        /// <returns>if succeeded return list of week otherwise returns an error</returns>
        Task<Response<IEnumerable<WeekResponse>>> GetWeeksByTraining(int trainingId);
        /// <summary>
        /// get a specific week
        /// </summary>
        /// <param name="weekId">id of the  week</param>
        /// <returns>if succeeded returns week data otherwise returns an error</returns>
        Task<Response<Week>> GetTheWeek(int weekId);
        /// <summary>
        /// delete a specific week
        /// </summary>
        /// <param name="weekId">id of the week</param>
        /// <returns>succeeded or failed</returns>
        Task<Response> deleteWeek(int weekId);
        /// <summary>
        /// link a sheet to a week
        /// </summary>
        /// <param name="weekId">id of the week</param>
        /// <param name="sheetId">id of the sheet</param>
        /// <returns>succeeded or failed</returns>
        Task<Response> LinkSheet(int weekId, int sheetId);
        /// <summary>
        /// unlink a sheet from a week
        /// </summary>
        /// <param name="weekId">id of the week</param>
        /// <param name="sheetId">id of the sheet</param>
        /// <returns>succeeded or failed</returns>
        Task<Response> UnLinkSheet(int weekId, int sheetId);
    }
}
