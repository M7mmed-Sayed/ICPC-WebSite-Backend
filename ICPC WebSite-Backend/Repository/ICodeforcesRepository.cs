using CodeforcesLibrary;

namespace ICPC_WebSite_Backend.Repository
{
    public interface ICodeforcesRepository
    {
        Task<List<Submission>> GetContestSubmissions(string contestId,string userCodeforcesHandle);
    }
}