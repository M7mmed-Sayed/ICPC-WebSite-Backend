using CodeforcesLibrary;

namespace ICPC_WebSite_Backend.Repository
{
    public class CodeforcesRepository : ICodeforcesRepository
    {
        private readonly CodeforcesHelper codeforcesHelper;

        public CodeforcesRepository(CodeforcesHelper codeforcesHelper)
        {
            this.codeforcesHelper = codeforcesHelper;
        }
        public async Task<List<ContestSubmissionsResponse>> GetContestSubmissions(string contestId,string userCodeforcesHandle)
        {
            var result= await codeforcesHelper.GetContestSubmissionsAsync(contestId, userCodeforcesHandle); ;
            return result;

        }
        public async Task<ContestStandings> GetContestStanding(string contestId)
        {
            var result = await codeforcesHelper.GetContestStandingAsync(contestId); ;
            return result;

        }
    }
}
