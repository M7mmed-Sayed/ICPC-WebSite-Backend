using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeforcesLibrary
{
    public class ContestSubmissions
    {
        public string Status { get; set; }
        [JsonProperty("Result")]
        public List<Submission> Submission { get; set; }
    }
}
