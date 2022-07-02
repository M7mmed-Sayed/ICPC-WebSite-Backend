using CodeforcesLibrary;
using UtilityLibrary.Response;

namespace ICPC_WebSite_Backend.Repository
{
    public class CodeforcesRepository : ICodeforcesRepository
    {
        private readonly CodeforcesHelper _codeforcesHelper;

        public CodeforcesRepository(CodeforcesHelper codeforcesHelper)
        {
            this._codeforcesHelper = codeforcesHelper;
        }
        public async Task<List<ContestSubmissionsResponse>> GetContestSubmissions(string contestId,string userCodeforcesHandle)
        {
            var result= await _codeforcesHelper.GetContestSubmissionsAsync(contestId, userCodeforcesHandle); ;
            return result;
        }
        public async Task<Response<ContestStandings>> GetContestStanding(string contestId)
        {
            var result = await _codeforcesHelper.GetContestStandingAsync(contestId); ;
            return result;

        }
    }
}
