using CodeforcesLibrary;
using UtilityLibrary.Response;

namespace ICPC_WebSite_Backend.Repository
{
    public interface ICodeforcesRepository
    {
        /// <summary>
        /// get all the sumbimssions of some user from a contest on codeforces
        /// </summary>
        /// <param name="contestId">the id of the contest</param>
        /// <param name="userCodeforcesHandle">the handle of the user on codeforces</param>
        /// <returns>returns list of contest submissions respone</returns>
        Task<List<ContestSubmissionsResponse>> GetContestSubmissions(string contestId,string userCodeforcesHandle);
        /// <summary>
        /// get the standing of a contest on codeforces
        /// </summary>
        /// <param name="contestId">the id of the contest</param>
        /// <returns>returns the standing of the contest</returns>
        Task<Response<ContestStandings>> GetContestStanding(string contestId);
    }
}