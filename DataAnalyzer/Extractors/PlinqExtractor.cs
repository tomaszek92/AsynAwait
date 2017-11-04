using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AsynchronousProgramming.DataAnalyzer.Extractors
{
    public class PlinqExtractor : IExtractor
    {
        public Dictionary<int, List<int>> Extract(string path)
        {
            var resDic = File.ReadLines(path)
                .AsParallel()
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

            return resDic;
        }
    }
}