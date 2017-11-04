using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AsynchronousProgramming.DataAnalyzer.Extractors;

namespace AsynchronousProgramming.DataAnalyzer
{
    static class Program
    {
        private static string _path = @"C:\Users\Tomasz Tomczykiewicz\Desktop\data.txt";

        static void Main(string[] args)
        {
            TestAll();
            //Test();
            Console.ReadKey();
        }

        static void TestAll()
        {
            var extractors = new Dictionary<IExtractor, string>
            {
                {new SynchronousExtractor(), "SynchronousExtractor"},
                {new ParallelV1Extractor(), "ParallelV1Extractor"},
                {new ParallelV2Extractor(), "ParallelV2Extractor"},
                {new ParallelV3Extractor(), "ParallelV3Extractor"},
                {new ParallelV4Extractor(), "ParallelV4Extractor"}
            };
            foreach (var extractor in extractors)
            {
                TestSingle(extractor.Key, extractor.Value);
            }
        }

        static void TestSingle(IExtractor extractor, string name)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            extractor.Extract(_path);
            stopwatch.Stop();
            Console.WriteLine($"{name}: {stopwatch.Elapsed.TotalSeconds:F}");
        }

        static void Test()
        {
            IExtractor extractor = new ParallelV4Extractor();

            Stopwatch stopwatch = Stopwatch.StartNew();
            var dictionary = extractor.Extract(_path);
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