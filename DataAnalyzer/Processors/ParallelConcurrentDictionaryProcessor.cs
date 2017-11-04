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
            int linesPerTask = lines.Length / Environment.ProcessorCount;
            List<Task> tasks = new List<Task>();

            for (int i = 0; i < Environment.ProcessorCount; i++)
            {
                int localI = i;
                Task task = Task.Run(() =>
                {
                    int startLine = localI * linesPerTask;
                    string[] taskLines = lines
                        .Skip(startLine)
                        .Take(linesPerTask)
                        .ToArray();
                    foreach (string line in taskLines)
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