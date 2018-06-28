using System.Collections.Generic;

namespace day22
{
    class Cluster : ClusterBase
    {
        public Cluster(string[] lines) : base(lines)
        {
        }

        public override bool Burst()
        {
            var packedPos = Pack(_x, _y);
            var isInfected = _infected.TryGetValue(packedPos, out var state) && state == State.Infected;

            // 1. If the current node is infected, it turns to its right. Otherwise, it turns to its left.
            // 2. toggle
            if (isInfected)
            {
                _infected.Remove(packedPos);
                Right();
            }
            else
            {
                _infected.Add(packedPos, State.Infected);
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