namespace CodeforcesLibrary
{
    public class Utilities
    {
        public static string GenerateSixDigts() {
            var rnd = new Random();
            string ret = rnd.Next(0, (int)1e6 - 1).ToString();
            ret = ret.PadLeft(6, '0');
            return ret;
        }
    }
}
