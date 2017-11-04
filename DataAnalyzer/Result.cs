namespace AsynchronousProgramming.DataAnalyzer
{
    public class Result
    {
        public int UserId { get; }
        public double Average { get; }
        public int Count { get; }

        public Result(int userId, double average, int count)
        {
            UserId = userId;
            Average = average;
            Count = count;
        }

        public override string ToString()
        {
            return $"{nameof(UserId)}: {UserId}, {nameof(Average)}: {Average:F}, {nameof(Count)}: {Count}";
        }
    }
}