using System.Collections.Generic;

namespace AsynchronousProgramming.DataAnalyzer.Processors
{
    public interface IProcessor
    {
        Dictionary<int, List<int>> Process(string[] lines);
    }
}