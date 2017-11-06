using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AsynchronousProgramming.DataAnalyzer.Processors
{
    public class ParallelLockProcessor : IProcessor
    {
        public Dictionary<int, List<int>> Process(string[] lines)
        {
            Dictionary<int, List<int>> dic = new Dictionary<int, List<int>>();
            var linesSplitted = ExtractorHelper.GetLinesSplitted(lines, Environment.ProcessorCount);
            object locker = new object();
            List<Task> tasks = new List<Task>();

            for (int i = 0; i < Environment.ProcessorCount; i++)
            {
                int localI = i;
                Task task = Task.Run(() =>
                {
                    foreach (string line in linesSplitted[localI])
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