using System.Collections.Generic;
using System.Linq;

namespace AsynchronousProgramming.DataAnalyzer.Processors
{
    public class LinqProcessor : IProcessor
    {
        public Dictionary<int, List<int>> Process(string[] lines)
        {
            return lines
                .Select(line =>
                {
                    var (userId, rate) = ExtractorHelper.GetLineInfo(line);
                    return new
                    {
                        UserId = userId,
                        Rate = rate
                    };
                })
                .GroupBy(res => res.UserId)
                .ToDictionary(
                    group => group.Key,
                    group => group.Select(res => res.Rate).ToList());
        }
    }
}