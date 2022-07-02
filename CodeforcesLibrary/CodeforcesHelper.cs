using System.Security.Cryptography;
using System.Text;
using UtilityLibrary.Response;
using UtilityLibrary.Utility;

namespace CodeforcesLibrary
{
    public class CodeforcesHelper
    {
        private readonly string _key;
        private readonly string _secret;
        private readonly string _baseUrl;

        public CodeforcesHelper(string baseUrl, string key, string secret) {
            this._key = key;
            this._secret = secret;
            this._baseUrl = baseUrl;
            ApiHelper.InitializeClient();
        }
        public async Task<Response<ContestStandings>> GetContestStandingAsync(string contestId) {
            string url = ConstructApiUrl("contest.standings", contestId);
            Console.WriteLine(url);
            using (var response = await ApiHelper.ApiClient.GetAsync(url)) {
                if (!response.IsSuccessStatusCode) {
                    return ResponseFactory.Fail<ContestStandings>(ErrorsList.CodeforcesFetchError(response.StatusCode.ToString()));
                }
                var responseContent = await response.Content.ReadAsAsync<ContestStandings>();
                return ResponseFactory.Ok<ContestStandings>(responseContent);
            }
        }
        public async Task<List<ContestSubmissionsResponse>> GetContestSubmissionsAsync(string contestId, string userCodeforcesHandle) {
            string url = ConstructApiUrl("contest.status", contestId, userCodeforcesHandle);
            using (var response = await ApiHelper.ApiClient.GetAsync(url)) {
                if (response.IsSuccessStatusCode) {
                    var responseContent = await response.Content.ReadAsAsync<ContestSubmissions>();
                    return responseContent.Response;
                }
                else {
                    throw new Exception($"CodeForces Failed with status code = {response.StatusCode}");
                }
            }
        }
        public string ConstructApiUrl(string methodName, string contestId, string userCodeforcesHandle = "") {
            var dateTimeUnix = DateTimeOffset.Now.ToUnixTimeSeconds();
            var methodApiUrl = $"{methodName}?apiKey={_key}&contestId={contestId}&handle={userCodeforcesHandle}&time={dateTimeUnix}";
            var url = _baseUrl + methodApiUrl;
            var randSixDigits = Utilities.GenerateSixDigts();
            string apiSig = $"{randSixDigits}/{methodApiUrl}#{_secret}";
            string apiHash = HashSha512(apiSig).ToLower();
            url += $"&apiSig={randSixDigits}" + apiHash;
            return url;
        }

        string HashSha512(string source) {
            using (SHA512 sha512Hash = SHA512.Create()) {
                //From String to byte array
                byte[] sourceBytes = Encoding.UTF8.GetBytes(source);
                byte[] hashBytes = sha512Hash.ComputeHash(sourceBytes);
                string hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty);
                return hash;
            }
        }
    }
}
