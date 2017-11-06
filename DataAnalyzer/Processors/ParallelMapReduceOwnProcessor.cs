using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AsynchronousProgramming.DataAnalyzer.Processors
{
    public class ParallelMapReduceOwnProcessor : IProcessor
    {
        public Dictionary<int, List<int>> Process(string[] lines)
        {
            var tasks = new List<Task<Dictionary<int, List<int>>>>();
            var linesSplitted = ExtractorHelper.GetLinesSplitted(lines, Environment.ProcessorCount);

            for (int i = 0; i < Environment.ProcessorCount; i++)
            {
                int localI = i;
                Task<Dictionary<int, List<int>>> task = Task.Run(() =>
                {
                    var dic = new Dictionary<int, List<int>>();
                    foreach (string line in linesSplitted[localI])
                    {
                        var (userId, rate) = ExtractorHelper.GetLineInfo(line);
                        if (dic.TryGetValue(userId, out var rates))
                        {
                            rates.Add(rate);
                        }
                        else
                        {
                            dic[userId] = new List<int>(200) {rate};
                        }
                    }
                    return dic;
                });
                tasks.Add(task);
            }
            Task.WaitAll(tasks.ToArray());

            var resDic = new Dictionary<int, List<int>>();
            foreach (var dic in tasks.Select(task => task.Result))
            {
                foreach (var pair in dic)
                {
                    if (resDic.ContainsKey(pair.Key))
                    {
                        resDic[pair.Key].AddRange(pair.Value);
                    }
                    else
                    {
                        resDic[pair.Key] = pair.Value;
                    }
                }
            }

            return resDic;
        }
    }
}