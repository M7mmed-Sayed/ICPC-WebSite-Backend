using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeforcesLibrary
{
    public class Author
    {
        public string ContestId { get; set; }
        public List<Member> Members { get; set; }
        public string ParticipantType { get; set; }
        public string TeamId { get; set; }
        public string TeamName { get; set; }
    }
}
