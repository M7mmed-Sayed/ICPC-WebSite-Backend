using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeforcesLibrary
{
    public class RankRow
    {
        [JsonProperty("party")]
        public Author Members { get; set; }
        public string rank { get; set; }
        public string points { get; set; }
        public string penalty { get; set; }
       
 
    }
}
