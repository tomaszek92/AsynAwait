using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AsynchronousProgramming.DataAnalyzer.Extractors
{
    public class ParallelV4Extractor : IExtractor
    {
        public Dictionary<int, List<int>> Extract(string path)
        {
            var resDic = new Dictionary<int, List<int>>();
            Parallel.ForEach(
                File.ReadAllLines(path),
                () => new Dictionary<int, List<int>>(),
                (line, state, localDic) =>
                {
                    var (userId, rate) = ExtractorHelper.GetLineInfo(line);
                    if (localDic.TryGetValue(userId, out var rates))
                    {
                        rates.Add(rate);
                    }
                    else
                    {
                        localDic[userId] = new List<int>(200) {rate};
                    }
                    return localDic;
                },
                localDic =>
                {
                    lock (resDic)
                    {
                        foreach (var pair in localDic)
                        {
                            if (resDic.ContainsKey(pair.Key))
                            {
                                resDic[pair.Key].AddRange(pair.Value);
                            }
                            else
                            {
                                //resDic[pair.Key] = new List<int>(200);
                                //resDic[pair.Key].AddRange(pair.Value);
                                resDic[pair.Key] = pair.Value;
                            }
                        }
                    }
                });
            return resDic;
        }
    }
}