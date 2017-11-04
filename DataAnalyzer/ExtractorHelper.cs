namespace AsynchronousProgramming.DataAnalyzer
{
    public class ExtractorHelper
    {
        public static (int userId, int rate) GetLineInfo(string line)
        {
            int userId = int.Parse(line.Substring(0, 5));
            int rate = int.Parse(line.Substring(6, 1));
            return (userId, rate);
        }
    }
}