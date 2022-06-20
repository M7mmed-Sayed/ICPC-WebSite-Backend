using Newtonsoft.Json;

namespace CodeforcesLibrary
{
    public class RankRow
    {
        [JsonProperty("party")]
        public Author Members { get; set; }
        public string Rank { get; set; }
        public string Points { get; set; }
        public string Penalty { get; set; }
       
 
    }
}
