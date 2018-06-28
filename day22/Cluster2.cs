namespace day22
{
    class Cluster2 : ClusterBase
    {
        public Cluster2(string[] lines) : base(lines)
        {
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

            Move();

            return !isNodeInfected;
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