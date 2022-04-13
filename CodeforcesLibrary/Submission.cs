using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeforcesLibrary
{
    public class Submission
    {
        public int Id { get; set; }
        public int ContestId { get; set; }
        public Problem Problem { get; set; }
        public Author Author { get; set; }
        public string ProgrammingLanguage { get; set; }
        public string Verdict { get; set; }
    }
}
