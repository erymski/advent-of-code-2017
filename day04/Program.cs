using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day04
{
    class Program
    {
        static void Main(string[] args)
        {
            int invalid = 0;

            var dataPath = Path.Combine(Environment.CurrentDirectory, @"..\..", "input.txt");
            var lines = File.ReadAllLines(dataPath);

            foreach (var line in lines)
            {
                var set = new HashSet<string>();

                // task #2
                foreach (var word in line.Split())
                {
                    // sort chars in the string alphabetically
                    var candidate = string.Join(string.Empty, word.OrderBy(c => c).ToArray());
                    if (set.Contains(candidate))
                    {
                        invalid++;
                        break;
                    }

                    set.Add(candidate);
                }

                // task #1
                //foreach (var word in line.Split())
                //{
                //    if (set.Contains(word))
                //    {
                //        //Console.WriteLine(line);
                //        invalid++;
                //        break;
                //    }

                //    set.Add(word);
                //}
            }

            var result = lines.Length - invalid;

            Console.WriteLine(result);
            Console.ReadLine();
        }
    }
}
