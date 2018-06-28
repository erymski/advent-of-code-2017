using System;
using System.IO;

namespace day22
{
    class Program
    {
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

            int infected = Part1(lines);
            Console.WriteLine($"Infected count = {infected}");


            Console.ReadLine();
        }

        private static int Part1(string[] lines)
        {
            int infected = 0;
            var cluster = new Cluster(lines);
            for (int i = 0; i < 10000; i++)
            {
//                cluster.Dump();

                if (cluster.Burst())
                {
                    infected++;
                }

//                Console.WriteLine();
            }

            return infected;
        }
    }
}
