using System.Text.RegularExpressions;

namespace ICPC_WebSite_Backend.Utility
{
    public class RegexPattrens
    {
        public static Regex Name = new Regex("^[a-zA-Z]*$");
        public static Regex Username = new Regex("^[a-zA-Z0-9]*$");

        //password
        public static Regex HasNumber = new Regex(@"[0-9]+");
        public static Regex HasUpperChar = new Regex(@"[A-Z]+");
        public static Regex HasLowerChar = new Regex(@"[a-z]+");
        public static Regex HasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");
        public static Regex HasMinimum8Chars = new Regex(@".{8,}");

    }
}
