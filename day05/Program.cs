using System;
using System.IO;
using System.Linq;

namespace day05
{
    class Program
    {
        static void Main(string[] args)
        {
            // int[] steps = {0, 3, 0, 1, -3};
            var dataPath = Path.Combine(Environment.CurrentDirectory, @"..\..", "input.txt");
            var steps = File.ReadAllLines(dataPath).Select(int.Parse).ToArray();

            var count = Task1((int[]) steps.Clone());
            Console.WriteLine(count);

            var count2 = Task2((int[])steps.Clone());
            Console.WriteLine(count2);

            Console.ReadLine();
        }

        private static int Task1(int[] steps)
        {
            int pos = 0, count = 0;
            while (true)
            {
                if (pos >= steps.Length || pos < 0) break;

                // jump
                int offset = steps[pos];
                steps[pos]++;
                pos += offset;

                count++;
            }
            return count;
        }

        private static int Task2(int[] steps)
        {
            int pos = 0, count = 0;
            while (true)
            {
                if (pos >= steps.Length || pos < 0) break;

                // jump
                int offset = steps[pos];
                if (offset >= 3)
                {
                    steps[pos]--;
                }
                else
                {
                    steps[pos]++;
                }
                pos += offset;

                count++;
            }
            return count;
        }
    }
}
