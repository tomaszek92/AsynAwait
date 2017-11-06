using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AsynchronousProgramming.DataAnalyzer.Processors
{
    public class ParallelConcurrentDictionaryProcessor : IProcessor
    {
        public Dictionary<int, List<int>> Process(string[] lines)
        {
            ConcurrentDictionary<int, List<int>> dic = new ConcurrentDictionary<int, List<int>>();
            var linesSplitted = ExtractorHelper.GetLinesSplitted(lines, Environment.ProcessorCount);
            List<Task> tasks = new List<Task>();

            for (int i = 0; i < Environment.ProcessorCount; i++)
            {
                int localI = i;
                Task task = Task.Run(() =>
                {
                    foreach (string line in linesSplitted[localI])
                    {
                        var (userId, rate) = ExtractorHelper.GetLineInfo(line);
                        dic.AddOrUpdate(userId, new List<int>(200) {rate}, (user, rates) =>
                        {
                            rates.Add(rate);
                            return rates;
                        });
                    }
                });
                tasks.Add(task);
            }
            Task.WaitAll(tasks.ToArray());

            return dic.ToDictionary(pair => pair.Key, pair => pair.Value);
        }
    }
}