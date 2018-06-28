using System.Collections.Generic;

namespace day22
{
    class Cluster : ClusterBase
    {
        // no need to allocate memory for the whole cluster, just keep items with values
        private readonly HashSet<int> _infected = new HashSet<int>();

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

        public bool Burst()
        {
            var packedPos = Pack(_x, _y);
            var isInfected = _infected.Contains(packedPos);

            // 1. If the current node is infected, it turns to its right. Otherwise, it turns to its left.
            // 2. toggle
            if (isInfected)
            {
                _infected.Remove(packedPos);
                Right();
            }
            else
            {
                _infected.Add(packedPos);
                Left();
            }

            // move
            Move();

            return !isInfected;
        }

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