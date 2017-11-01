using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace AsynchronousProgramming.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //ThreadsVsTasks();
            ManyTasks();
            //TaskAwaitAsync().Wait();

            Console.ReadKey();
        }

        static void ThreadsVsTasks()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < 10; i++)
            {
                var thread = new Thread(() => { });
                thread.Start();
            }
            stopwatch.Stop();
            Console.WriteLine($"Thread: {stopwatch.ElapsedMilliseconds}");

            stopwatch.Restart();
            for (int i = 0; i < 10; i++)
            {
                var task = Task.Run(() => { });
            }
            stopwatch.Stop();
            Console.WriteLine($"Task: {stopwatch.ElapsedMilliseconds}");
        }

        static void ManyTasks()
        {
            int cores = Environment.ProcessorCount;
            Console.WriteLine($"ProcessorCount: {cores}");

            int tasksToRunCount = 100;
            var tasks = new List<Task>();
            int number;
            for (number = 0; number < cores; number++)
            {
                tasks.Add(Operation(number));
            }
            tasksToRunCount -= cores;

            while (tasks.Count > 0)
            {
                int index = Task.WaitAny(tasks.ToArray());
                tasks.RemoveAt(index);

                if (tasksToRunCount > 0)
                {
                    tasks.Add(Operation(++number));
                    tasksToRunCount--;
                }
            }
        }

        private static Task Operation(int number)
        {
            return Task.Run(() =>
            {
                Console.WriteLine($"Starting task {number}");
                Stopwatch stopwatch = Stopwatch.StartNew();
                while (stopwatch.ElapsedMilliseconds < 10)
                {
                }
                Console.WriteLine($"Ending task {number}");
            });
        }

        static async Task TaskAwaitAsync()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            //await TestAsync(6000);
            //await TestAsync(1000);
            //stopwatch.Stop();
            //Console.WriteLine(stopwatch.ElapsedMilliseconds);

            Task test1 = DelayWithWriteToConsoleAsync(6000);
            Task test2 = DelayWithWriteToConsoleAsync(1000);

            await Task.WhenAll(test1, test2);
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds);
        }

        static async Task DelayWithWriteToConsoleAsync(int ms)
        {
            await Task.Delay(ms);
            Console.WriteLine($"Done after {ms}");
        }
    }
}