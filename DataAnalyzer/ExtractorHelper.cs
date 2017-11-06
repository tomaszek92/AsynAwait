using System.Collections.Generic;

namespace AsynchronousProgramming.DataAnalyzer
{
    public static class ExtractorHelper
    {
        public static (int userId, int rate) GetLineInfo(string line)
        {
            int userId = int.Parse(line.Substring(0, 5));
            int rate = int.Parse(line.Substring(6, 1));
            return (userId, rate);
        }

        public static List<string[]> GetLinesSplitted(string[] lines, int tasksCount)
        {
            int linesPerTask = lines.Length / tasksCount;
            var result = new List<string[]>(tasksCount);
            for (int i = 0; i < tasksCount; i++)
            {
                result.Add(new string[linesPerTask]);
                int j = 0;
                int startLine = i * linesPerTask;
                int endLine = startLine + linesPerTask;
                for (int line = startLine; line < endLine; line++)
                {
                    result[i][j++] = lines[line];
                }
            }
            return result;
        }
    }
}