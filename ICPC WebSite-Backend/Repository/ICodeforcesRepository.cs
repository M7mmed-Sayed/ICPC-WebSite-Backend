using CodeforcesLibrary;
using UtilityLibrary.Response;

namespace ICPC_WebSite_Backend.Repository
{
    public interface ICodeforcesRepository
    {
        Task<List<ContestSubmissionsResponse>> GetContestSubmissions(string contestId,string userCodeforcesHandle);
        Task<Response<ContestStandings>> GetContestStanding(string contestId);
    }
}