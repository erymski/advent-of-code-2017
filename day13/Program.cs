using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day13
{
    class Program
    {
        private static readonly string[] input = {"0: 3", "1: 2", "4: 4", "6: 4"};
        private static readonly char[] Separator = " :".ToCharArray();

        static void Main()
        {
            int maxIndex = 0;
            Dictionary<int, int> layerToRange = new Dictionary<int, int>(input.Length);

            // load data
            var dataPath = Path.Combine(Environment.CurrentDirectory, @"..\..", "input.txt");
            foreach (var line in /*input*/File.ReadAllLines(dataPath))
            {
                var pair = line
                            .Split(Separator, StringSplitOptions.RemoveEmptyEntries)
                            .Select(int.Parse)
                            .ToArray();

                var depth = pair[0];
                var range = pair[1];
                layerToRange.Add(depth, range);

                if (depth > maxIndex) maxIndex = depth;
            }



            var res1 = Task1(maxIndex, layerToRange);
            Console.WriteLine(res1);

            var res2 = Task2(maxIndex, layerToRange);
            Console.WriteLine(res2);

            Console.ReadLine();
        }

        private static int Task1(int maxIndex, Dictionary<int, int> layerToRange)
        {
            int hits = 0;
            int picoseconds;
            for (picoseconds = 0; picoseconds < maxIndex + 1; picoseconds++)
            {
                int range;
                if (layerToRange.TryGetValue(picoseconds, out range))
                {
                    // calc loop
                    // 2 -> 2; 3 -> 4; 4 -> 6
                    int loop = (range - 1) * 2;
                    var pos = picoseconds % loop;
                    if (pos == 0)
                    {
                        hits += picoseconds * range;
                    }
                }
            }

            return hits;
        }

        private static int Task2(int maxIndex, Dictionary<int, int> layerToRange)
        {
            int delay = 0;
            while (true)
            {
                bool hits = true;
                int picoseconds;
                for (picoseconds = 0; picoseconds < maxIndex + 1; picoseconds++)
                {
                    int range;
                    if (layerToRange.TryGetValue(picoseconds, out range))
                    {
                        // calc loop
                        // 2 -> 2; 3 -> 4; 4 -> 6
                        int loop = (range - 1) * 2;
                        var corrected = picoseconds + delay;
                        var pos = corrected % loop;
                        if (pos == 0)
                        {
                            hits = false;
                            break;
                        }
                    }
                }
                if (hits) break;
                delay++;
            }

            return delay;
        }
    }
}
