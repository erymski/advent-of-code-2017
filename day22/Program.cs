using System;
using System.Collections.Generic;
using System.IO;

namespace day22
{
    struct Dir
    {
        public int dx;
        public int dy;
    }

    class Cluster
    {
        /// <summary>
        /// CCW directions: up, left, down, right.
        /// </summary>
        static readonly Dir[] _dirs =
        {
            new Dir { dx = 0, dy = 1 },
            new Dir { dx = 1, dy = 0 },
            new Dir { dx = 0, dy = -1 },
            new Dir { dx = -1, dy = 0 },
        };

        private static readonly int _dirCount = _dirs.Length;

        private int _x;
        private int _y;
        private int _rotateIndex;
        private readonly HashSet<int> _infected = new HashSet<int>();
        //private int _minX;
        //private int _minY;
        //private int _maxY;
        //private int _maxX;

        public Cluster(string[] lines)
        {
            int size = lines.Length;
            _x = size / 2;
            _y = size / 2;
            _rotateIndex = 0; // look up

            //_minX = _minY = 0;
            //_maxX = _maxY = size - 1;

            for (var y = 0; y < size; y++)
            {
                var line = lines[y];
                for (var x = 0; x < size; x++)
                {
                    char ch = line[x];
                    if (ch == '#')
                    {
                        Set(x, y);
                    }
                }
            }
        }

        private void Set(int x, int y)
        {
            _infected.Add(Pack(x, y));
        }

        /// <summary>
        /// Pack 2D coordinate to int
        /// </summary>
        private static int Pack(int x, int y) // long would be safer, but no need to input size
        {
            return (x << 16) + y;
        }

        public bool Burst()
        {
            var packedPos = Pack(_x, _y);
            var isNodeInfected = _infected.Contains(packedPos);

            // 1. If the current node is infected, it turns to its right. Otherwise, it turns to its left.
            // 2. toggle
            if (isNodeInfected)
            {
                _infected.Remove(packedPos);
                _rotateIndex++;
                _rotateIndex %= _dirCount;
            }
            else
            {
                _infected.Add(packedPos);
                _rotateIndex--;
                if (_rotateIndex < 0)
                {
                    _rotateIndex = _dirCount - 1;
                }
            }

            // move
            var dir = _dirs[_rotateIndex];
            _x += dir.dx;
            _y += dir.dy;
            //UpdateBounds();

            return !isNodeInfected;
        }

        //private void UpdateBounds()
        //{
        //    if (_x > _maxX)
        //    {
        //        _maxX = _x;
        //    }
        //    else if (_x < _minX)
        //    {
        //        _minX = _x;
        //    }

        //    if (_y > _maxY)
        //    {
        //        _maxY = _y;
        //    }
        //    else if (_y < _minY)
        //    {
        //        _minY = _y;
        //    }
        //}

        //public void Dump()
        //{
        //    for (int j = _minY; j <= _maxY; j++)
        //    {
        //        for (int i = _minX; i <= _maxX; i++)
        //        {
        //            if ((_x == i) && (_y == j))
        //            {
        //                Console.Write('x');
        //            }
        //            else
        //            {
        //                Console.Write(_infected.Contains(Pack(i, j)) ? '#' : '.');
        //            }
        //        }
        //        Console.WriteLine();
        //    }
        //}
    }

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


            Console.WriteLine($"Infected count = {infected}");
            Console.ReadLine();
        }
    }
}
