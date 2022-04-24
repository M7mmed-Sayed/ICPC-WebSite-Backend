using CodeforcesLibrary;

namespace ICPC_WebSite_Backend.Repository
{
    public interface ICodeforcesRepository
    {
        Task<List<ContestSubmissionsResponse>> GetContestSubmissions(string contestId,string userCodeforcesHandle);
        Task<ContestStandings> GetContestStanding(string contestId);
    }
}