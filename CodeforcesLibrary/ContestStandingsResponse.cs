using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
