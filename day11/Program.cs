using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

// ReSharper disable InconsistentNaming

namespace day11
{
    enum Dir
    {
        N = 0,
        NE,
        SE,
        S,
        SW,
        NW
    }
    class Program
    {
        private static readonly char[] Separator = ",".ToCharArray();

        static void Main(string[] args)
        {
            //int steps1 = Count("ne,ne,ne".Split(Separator).Select(step => (Dir)Enum.Parse(typeof(Dir), step, true))); // 3
            //int steps2 = Count("ne,ne,sw,sw".Split(Separator).Select(step => (Dir)Enum.Parse(typeof(Dir), step, true))); // 0
            //int steps3 = Count("ne,ne,s,s".Split(Separator).Select(step => (Dir)Enum.Parse(typeof(Dir), step, true))); // 2
            //int steps4 = Count("se,sw,se,sw,sw".Split(Separator).Select(step => (Dir)Enum.Parse(typeof(Dir), step, true))); // 3
            //int steps5 = Count("se,n,nw".Split(Separator).Select(step => (Dir)Enum.Parse(typeof(Dir), step, true))); // 1
            //int steps6 = Count("nw,s,se".Split(Separator).Select(step => (Dir)Enum.Parse(typeof(Dir), step, true))); // 1

            var dataPath = Path.Combine(Environment.CurrentDirectory, @"..\..", "input.txt");
            string input = File.ReadAllText(dataPath);
            var arr = input.Split(Separator).Select(step => (Dir)Enum.Parse(typeof(Dir), step, true)).ToArray();
            int max = 0;

            // brute force
            for (int i = arr.Length; i > 0; i--)
            {
                int steps = Count(arr.Take(i));
                if (i == arr.Length) // whole sequence of steps
                {
                    Console.WriteLine(steps);
                }
                if (steps > max)
                {
                    max = steps;
                }
            }
            Console.WriteLine(max);

            Console.ReadLine();
        }

        private static int Count(IEnumerable<Dir> arr)
        {
            int[] amounts = new int[6];
            foreach (var dir in arr)
            {
                amounts[(int)dir]++;
            }

            // remove opposite movements
            for (int i = 0; i < 3; i++)
            {
                int min = Math.Min(amounts[i], amounts[i + 3]);
                amounts[i] -= min;
                amounts[i + 3] -= min;
            }

            // straighten directions
            for (int i = 0; i < 6; i++)
            {
                int pairedIndex = (i + 2) % 6;
                int middleIndex = (i + 1) % 6;

                while (amounts[i] > 0 && amounts[pairedIndex] > 0)
                {
                    amounts[i]--;
                    amounts[pairedIndex]--;
                    amounts[middleIndex]++;
                }
            }

            return amounts.Sum();
        }
    }
}
