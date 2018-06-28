namespace day22
{
    internal class ClusterBase
    {
        protected int _x;
        protected int _y;
        protected int _rotateIndex;

        //private int _minX;
        //private int _minY;
        //private int _maxY;
        //private int _maxX;

        /// <summary>
        /// Pack 2D coordinate to int
        /// </summary>
        protected static int Pack(int x, int y) // long would be safer, but no need to input size
        {
            return (x << 16) + y;
        }

        protected void Move()
        {
            var dir = Dir.For(_rotateIndex);
            _x += dir.dx;
            _y += dir.dy;
            //UpdateBounds();
        }

        protected void Left()
        {
            _rotateIndex--;
            if (_rotateIndex < 0)
            {
                _rotateIndex = Dir.Count - 1;
            }
        }

        protected void Right()
        {
            _rotateIndex++;
            _rotateIndex %= Dir.Count;
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
    }
}