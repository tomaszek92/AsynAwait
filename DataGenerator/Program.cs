using System;
using System.IO;

namespace AsynchronousProgramming.DataGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Random rand = new Random();
            using (var file = File.Create(@"C:\Users\Tomasz Tomczykiewicz\Desktop\data.txt"))
            {
                using (StreamWriter streamWriter = new StreamWriter(file))
                {
                    for (int i = 0; i < 10000000; i++)
                    {
                        string line = $"{rand.Next(10000, 100000)} {rand.Next(1, 5)}";
                        streamWriter.WriteLine(line);
                    }
                }
            }
        }
    }
}