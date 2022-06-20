using Newtonsoft.Json;

namespace CodeforcesLibrary
{
    public class ContestSubmissions
    {
        public string Status { get; set; }
        [JsonProperty("Result")]
        public List<ContestSubmissionsResponse> response { get; set; }
    }
}
