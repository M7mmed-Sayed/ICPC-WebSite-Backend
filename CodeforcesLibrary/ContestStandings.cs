using Newtonsoft.Json;

namespace CodeforcesLibrary
{
    public class ContestStandings
    {
        public string status { get; set; }
        [JsonProperty("Result")]
        public ContestStandingsResponse response { get; set; }
    }
}
