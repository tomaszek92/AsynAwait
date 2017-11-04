using System.Collections.Generic;

namespace AsynchronousProgramming.DataAnalyzer
{
    public interface IProcessor
    {
        Dictionary<int, List<int>> Process(string[] lines);
    }
}