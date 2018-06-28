using System.Collections.Generic;

namespace day22
{
    class Cluster2
    {
        private int _x;
        private int _y;
        private int _rotateIndex;
        private readonly HashSet<int> _infected = new HashSet<int>();
        //private int _minX;
        //private int _minY;
        //private int _maxY;
        //private int _maxX;

        public Cluster2(string[] lines)
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
                _rotateIndex %= Dir.Count;
            }
            else
            {
                _infected.Add(packedPos);
                _rotateIndex--;
                if (_rotateIndex < 0)
                {
                    _rotateIndex = Dir.Count - 1;
                }
            }

            // move
            var dir = Dir.For(_rotateIndex);
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
}