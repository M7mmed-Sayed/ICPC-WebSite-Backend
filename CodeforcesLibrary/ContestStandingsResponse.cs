using Newtonsoft.Json;

namespace CodeforcesLibrary
{
    public class ContestStandingsResponse
    {
        public Contest Contest { get; set; }
        public List<Problem> Problems { get; set; }
        
        [JsonProperty("Rows")]
        public List<RankRow> RankRows { get; set; }
    }
}
