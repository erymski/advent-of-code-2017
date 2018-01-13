using System;
using System.IO;
using System.Linq;

namespace day20
{
    class Program
    {
        private static readonly char[] Separator = " ,pva<>=".ToCharArray();

        //private static readonly string[] input =
        //    {
        //        "p=< 3,0,0>, v=< 2,0,0>, a=<-1,0,0>",
        //        "p=< 4,0,0>, v=< 0,0,0>, a=<-2,0,0>"
        //    };


        static void Main()
        {
            var dataPath = Path.Combine(Environment.CurrentDirectory, @"..\..", "input.txt");
            var lines = File.ReadAllLines(dataPath);

            var particles = Parse(lines);

            Particle closest = Task1(particles);
            Console.WriteLine(closest.Index);

            int aliveCount = Task2(particles);
            Console.WriteLine(aliveCount);

            Console.ReadLine();
        }

        private static int Task2(Particle[] particles)
        {
            for (int iterations = 0; iterations < 50000; iterations++)
            {
                foreach (var particle in particles)
                {
                    if (particle.Alive)
                    {
                        particle.Step();
                    }
                }

                var pairs = particles
                                .Where(p => p.Alive)
                                .GroupBy(p => p.Hash())
                                .Select(group => new { count = group.Count(), group })
                                .OrderByDescending(group => group.count);

                foreach (var pair in pairs)
                {
                    if (pair.count == 1) break;

                    foreach (var particle in pair.group)
                    {
                        particle.Alive = false;
                    }
                }
            }

            // can be tracked in the original loop... even dynamics can be tracked
            return particles.Count(p => p.Alive);
        }

        private static Particle Task1(Particle[] particles)
        {
            // solving in bruteforce way
            // it should be possible to solve it more cleanly by finding direction and speed of moving out of origin,
            // but don't want to open textbooks

            for (int iterations = 0; iterations < 50000; iterations++)
            {
                foreach (var particle in particles)
                {
                    particle.Step();
                }
            }

            // can be tracked in the original loop... even dynamics can be tracked
            return particles.OrderBy(p => p.Distance).First();
        }

        private static Particle[] Parse(string[] lines)
        {
            return lines
                    // remove all chars, except numbers. Convert to ints
                    .Select(line => line.Split(Separator, StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray())
                    // Convert to particles
                    .Select((data, index) => new Particle(data, index))
                    .ToArray();
        }
    }
}
