using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CodeforcesLibrary
{
    public class CodeforcesHelper
    {
        private readonly string key;
        private readonly string secret;
        private readonly string baseUrl;

        public CodeforcesHelper(string key, string secret)
        {
            this.key = key;
            this.secret = secret;
            baseUrl = "https://codeforces.com/api/";
            ApiHelper.InitializeClient();
        }
        public async Task<ContestStandings> GetContestStandingAsync(string contestId)
        {
            string url = ConstructApiUrl("contest.standings",contestId);
            Console.WriteLine(url);
            using (var response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsAsync<ContestStandings>();
                    return responseContent;
                }
                else
                {
                    throw new Exception($"CodeForces Failed with status code = {response.StatusCode}");
                }
            }
        }
        public async Task<List<ContestSubmissionsResponse>> GetContestSubmissionsAsync(string contestId, string userCodeforcesHandle)
        {
            string url = ConstructApiUrl("contest.status",contestId, userCodeforcesHandle);
            using (var response = await ApiHelper.ApiClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsAsync<ContestSubmissions>();
                    return responseContent.response;
                }
                else
                {
                    throw new Exception($"CodeForces Failed with status code = {response.StatusCode}");
                }
            }
        }
        public string ConstructApiUrl(string methodName,string contestId,string userCodeforcesHandle="")
        {
            var dateTimeUnix = DateTimeOffset.Now.ToUnixTimeSeconds();
            var methodApiURL = $"{methodName}?apiKey={key}&contestId={contestId}&handle={userCodeforcesHandle}&time={dateTimeUnix}";
            var url = baseUrl+methodApiURL;
            var randSixDigits = GenerateSixDigts();
            string apiSig = $"{randSixDigits}/{methodApiURL}#{secret}";
            string apiHash = HashSHA512(apiSig).ToLower();
            url += $"&apiSig={randSixDigits}" + apiHash;
            return url;
        }
        public string GenerateSixDigts()
        {
            var rnd = new Random();
            string ret = rnd.Next(0,(int)1e6 - 1).ToString(); 
            ret=ret.PadLeft(6-ret.Length, '0');
            return ret;
        }
        string HashSHA512(string source)
        {
            using (SHA512 sha512Hash = SHA512.Create())
            {
                //From String to byte array
                byte[] sourceBytes = Encoding.UTF8.GetBytes(source);
                byte[] hashBytes = sha512Hash.ComputeHash(sourceBytes);
                string hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty);
                return hash;
            }
        }
    }
}
