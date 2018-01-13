using System;
using System.IO;
using System.Linq;

namespace day20
{
    struct Particle
    {
        /// <summary>
        /// Particle name
        /// </summary>
        public int Index { get; }

        // x,y,z,vx,vy,vz,ax,ay,az
        private readonly long[] _data;

        /// <summary>
        /// Distance from origin.
        /// </summary>
        public long Distance => Math.Abs(_data[0]) + Math.Abs(_data[1]) + Math.Abs(_data[2]);

        public Particle(long[] data, int index)
        {
            _data = data;
            Index = index;
        }

        public void Step()
        {
            SubStep(0); // x
            SubStep(1); // y
            SubStep(2); // z
        }

        private void SubStep(int index)
        {
            _data[index + 3] += _data[index + 6];
            _data[index] += _data[index + 3];
        }
    }

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

            var closest = particles.OrderBy(p => p.Distance).First(); // can be tracked in the original loop... even dynamics can be tracked
            Console.WriteLine(closest.Index);

            Console.ReadLine();
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
