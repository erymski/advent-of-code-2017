using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            //Particle closest = Task1(particles);
            //Console.WriteLine(closest.Index);

            Stopwatch watch = Stopwatch.StartNew();
            int aliveCount = Task2(particles);
            watch.Stop();
            Console.WriteLine(aliveCount + " " + watch.ElapsedMilliseconds);

            Console.ReadLine();
        }

        private static int Task2(Particle[] particles)
        {
            var total = particles.Length;
            int aliveCount = total;
            var collisions = new Dictionary<long, int>(aliveCount);
            for (int iterations = 0; iterations < 50000; iterations++)
            {
                collisions.Clear();
                for (var i = 0; i < total; i++)
                {
                    var particle = particles[i];
                    if (! particle.Alive) continue;

                    particle.Step();
                    var hash = particle.Hash();

                    int prevIndex;
                    if (collisions.TryGetValue(hash, out prevIndex))
                    {
                        var prevParticle = particles[prevIndex];
                        if (prevParticle.Alive)
                        {
                            prevParticle.Alive = false;
                            aliveCount--;
                        }
                        particle.Alive = false;
                        aliveCount--;
                    }
                    else
                    {
                        collisions.Add(hash, i);
                    }
                }
            }

            // can be tracked in the original loop... even dynamics can be tracked
            return aliveCount;
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
