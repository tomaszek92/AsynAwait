using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using AsynchronousProgramming.DataAnalyzer.Processors;

namespace AsynchronousProgramming.DataAnalyzer
{
    public static class Program
    {
        private static string _path = @"C:\Users\Tomasz Tomczykiewicz\Desktop\data.txt";

        private static void Main()
        {
            TestAll();
            //Test();
            Console.ReadKey();
        }

        private static void TestAll()
        {
            var extractors = new Dictionary<IProcessor, string>
            {
                {new LinqProcessor(), "LinqProcessor"},
                {new SynchronousProcessor(), "SynchronousProcessor"},
                {new ParallelLockProcessor(), "ParallelLockProcessor"},
                {new ParallelConcurrentDictionaryProcessor(), "ParallelConcurrentDictionaryProcessor"},
                {new ParallelMapReduceOwnProcessor(), "ParallelMapReduceOwnProcessor"},
                {new ParallelMapReduceProcessor(), "ParallelMapReduceProcessor"},
                {new PlinqProcessor(), "PlinqProcessor"}
            };
            var allLines = File.ReadAllLines(_path);
            foreach (var extractor in extractors)
            {
                TestSingle(extractor.Key, extractor.Value, allLines);
            }
        }

        private static void TestSingle(IProcessor processor, string name, string[] allLines)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            processor.Process(allLines);
            stopwatch.Stop();
            Console.WriteLine($"{name}: {stopwatch.Elapsed.TotalSeconds:F}");
        }

        private static void Test()
        {
            IProcessor processor = new LinqProcessor();

            var allLines = File.ReadAllLines(_path);
            Stopwatch stopwatch = Stopwatch.StartNew();
            var dictionary = processor.Process(allLines);
            stopwatch.Stop();

            Console.WriteLine($"{stopwatch.Elapsed.TotalSeconds:F}");
            Console.WriteLine();

            foreach (var result in dictionary
                .Select(pair => new Result(pair.Key, pair.Value.Average(), pair.Value.Count))
                .OrderByDescending(r => r.Count)
                .Take(10))
            {
                Console.WriteLine(result);
            }
        }
    }
}