using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AsynchronousProgramming.DataAnalyzer.Extractors
{
    public class ParallelV1Extractor : IExtractor
    {
        public Dictionary<int, List<int>> Extract(string path)
        {
            Dictionary<int, List<int>> dic = new Dictionary<int, List<int>>();
            int linesCount = File.ReadLines(path).Count();
            int linesPerTask = linesCount / Environment.ProcessorCount;
            object locker = new object();
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
                        lock (locker)
                        {
                            if (dic.TryGetValue(userId, out var rates))
                            {
                                rates.Add(rate);
                            }
                            else
                            {
                                dic[userId] = new List<int>(200) {rate};
                            }
                        }
                    }
                });
                tasks.Add(task);
            }
            Task.WaitAll(tasks.ToArray());

            return dic;
        }
    }
}