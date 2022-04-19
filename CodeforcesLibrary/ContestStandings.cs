using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeforcesLibrary
{
    public class ContestStandings
    {
        public string status { get; set; }
        [JsonProperty("Result")]
        public ContestStandingsResponse response { get; set; }
    }
}
