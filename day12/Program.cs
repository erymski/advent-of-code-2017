using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day12
{
    class Program
    {
        //private static string[] input = {
        //                                    "0 <-> 2",
        //                                    "1 <-> 1",
        //                                    "2 <-> 0, 3, 4",
        //                                    "3 <-> 2, 4",
        //                                    "4 <-> 2, 3, 6",
        //                                    "5 <-> 6",
        //                                    "6 <-> 4, 5"
        //                                };

        private static readonly char[] Separator = " <->,".ToCharArray();

        static void Main(string[] args)
        {
            var dataPath = Path.Combine(Environment.CurrentDirectory, @"..\..", "input.txt");
            string[] input = File.ReadAllLines(dataPath);

                // generate map for program groups (ID -> connected IDs)
            var map = new Dictionary<int, IEnumerable<int>>();
            foreach (var line in input)
            {
                var pieces = line.Split(Separator, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
                map.Add(pieces[0], pieces.Skip(1));
            }

            var res1 = Task1(map);
            Console.WriteLine(res1);

            var res2 = Task2(map);
            Console.WriteLine(res2);

            Console.ReadLine();
        }

        private static int Task1(Dictionary<int, IEnumerable<int>> map)
        {
            var processed = new HashSet<int>();

            var queue = new Queue<int>();
            queue.Enqueue(0); // look for group with '0;
            while (queue.Count > 0)
            {
                int id = queue.Dequeue();
                processed.Add(id);
                IEnumerable<int> deps = map[id].Where(i => !processed.Contains(i));
                foreach (var dep in deps)
                {
                    queue.Enqueue(dep);
                }
            }

            return processed.Count;
        }

        private static int Task2(Dictionary<int, IEnumerable<int>> map)
        {
            var processed = new HashSet<int>();
            var queue = new Queue<int>();
            int groups = 0;

            while (true)
            {
                var diff = map.Keys.Except(processed).ToArray(); // yuck, but OK for small lengths
                if (diff.Length == 0) break;

                groups++;

                queue.Enqueue(diff.First());
                while (queue.Count > 0)
                {
                    int id = queue.Dequeue();
                    processed.Add(id);
                    IEnumerable<int> deps = map[id].Where(i => !processed.Contains(i));
                    foreach (var dep in deps)
                    {
                        queue.Enqueue(dep);
                    }
                }
            }

            return groups;
        }
    }
}
