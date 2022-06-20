using Newtonsoft.Json;

namespace CodeforcesLibrary
{
    public class ContestStandings
    {
        public string Status { get; set; }
        [JsonProperty("Result")]
        public ContestStandingsResponse Response { get; set; }
    }
}
