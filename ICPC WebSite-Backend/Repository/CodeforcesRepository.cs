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
        public async Task<List<Submission>> GetContestSubmissions(string contestId,string userCodeforcesHandle)
        {
            var result= await codeforcesHelper.GetContestSubmissionsAsync(contestId, userCodeforcesHandle); ;
            return result;

        }
    }
}
