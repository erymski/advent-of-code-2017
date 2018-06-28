using System;
using System.IO;

namespace day22
{
    class Program
    {
        private const int IterationCount = 10000000;

        private static readonly string[] _test =
        {
            "..#",
            "#..",
            "..."
        };

        static void Main(string[] args)
        {
            var dataPath = Path.Combine(Environment.CurrentDirectory, @"..\..", "input.txt");
            var lines = File.ReadAllLines(dataPath)/*_test*/;
            Array.Reverse(lines); // inverted for easier debugging

            Run(new Cluster(lines));
            Run(new Cluster2(lines));

            Console.ReadLine();
        }

        private static void Run(ClusterBase cluster)
        {
            int infected = 0;
            for (int i = 0; i < IterationCount; i++)
            {
//                cluster.Dump();

                if (cluster.Burst())
                {
                    infected++;
                }

//                Console.WriteLine();
            }
            Console.WriteLine($"Infected count = {infected}");
        }
    }
}
