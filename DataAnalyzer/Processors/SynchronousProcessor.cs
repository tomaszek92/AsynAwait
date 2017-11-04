using System.Collections.Generic;

namespace AsynchronousProgramming.DataAnalyzer.Processors
{
    public class SynchronousProcessor : IProcessor
    {
        public Dictionary<int, List<int>> Process(string[] lines)
        {
            Dictionary<int, List<int>> dic = new Dictionary<int, List<int>>();
            foreach (string line in lines)
            {
                var (userId, rate) = ExtractorHelper.GetLineInfo(line);
                if (dic.TryGetValue(userId, out var rates))
                {
                    rates.Add(rate);
                }
                else
                {
                    dic[userId] = new List<int>(100) {rate};
                }
            }
            return dic;
        }
    }
}