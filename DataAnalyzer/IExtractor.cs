using System.Collections.Generic;

namespace AsynchronousProgramming.DataAnalyzer
{
    public interface IExtractor
    {
        Dictionary<int, List<int>> Extract(string path);
    }
}