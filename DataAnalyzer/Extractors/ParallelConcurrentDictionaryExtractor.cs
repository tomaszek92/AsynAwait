using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AsynchronousProgramming.DataAnalyzer.Extractors
{
    public class ParallelConcurrentDictionaryExtractor : IExtractor
    {
        public Dictionary<int, List<int>> Extract(string path)
        {
            ConcurrentDictionary<int, List<int>> dic = new ConcurrentDictionary<int, List<int>>();
            int linesCount = File.ReadLines(path).Count();
            int linesPerTask = linesCount / Environment.ProcessorCount;
            List<Task> tasks = new List<Task>();

            for (int i = 0; i < Environment.ProcessorCount; i++)
            {
                int localI = i;
                Task task = Task.Run(() =>
                {
                    int startLine = localI * linesPerTask;
                    string[] lines = File.ReadLines(path)
                        .Skip(startLine)
                        .Take(linesPerTask)
                        .ToArray();
                    foreach (string line in lines)
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