using System.Collections.Generic;
using System.IO;

namespace AsynchronousProgramming.DataAnalyzer.Extractors
{
    public class SynchronousExtractor : IExtractor
    {
        public Dictionary<int, List<int>> Extract(string path)
        {
            Dictionary<int, List<int>> dic = new Dictionary<int, List<int>>();
            using (FileStream fileStream = File.OpenRead(path))
            {
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
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
                }
            }
            return dic;
        }
    }
}