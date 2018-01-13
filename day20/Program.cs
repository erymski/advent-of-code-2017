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

            Particle closest = Task1(ToParticles(lines));
            Console.WriteLine(closest.Index);

            Particle[] particles = ToParticles(lines);
//            Stopwatch watch = Stopwatch.StartNew();
            int aliveCount = Task2(particles);
//            watch.Stop();
            Console.WriteLine(aliveCount/* + " " + watch.ElapsedMilliseconds*/);

            Console.ReadLine();
        }

        private static int Task2(Particle[] particles)
        {
            var total = particles.Length;
            int aliveCount = total;
            for (int iterations = 0; iterations < 50000; iterations++)
            {
                var collisions = new Dictionary<long, int>(aliveCount); // faster to recreate it each time

                for (var i = 0; i < total; i++)
                {
                    var particle = particles[i];
                    if (! particle.Alive) continue;

                    particle.Step();
                    var hash = particle.Hash();

                    // see if any other particle has the same hash
                    int prevIndex;
                    if (collisions.TryGetValue(hash, out prevIndex))
                    {
                        // mark both particles as dead

                        if (particles[prevIndex].Alive)
                        {
                            particles[prevIndex].Alive = false;
                            aliveCount--;
                        }
                        particle.Alive = false;
                        aliveCount--;
                    }
                    else
                    {
                        // store the hash
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

        private static Particle[] ToParticles(string[] lines)
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
