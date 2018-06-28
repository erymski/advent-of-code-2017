using System;
using System.Collections.Generic;

namespace day22
{
    internal abstract class ClusterBase
    {
        protected int _x;
        protected int _y;
        protected int _rotateIndex;

        private int _minX;
        private int _minY;
        private int _maxY;
        private int _maxX;

        // no need to allocate memory for the whole cluster, just keep items with values
        protected readonly Dictionary<int, State> _infected = new Dictionary<int, State>();

        protected ClusterBase(string[] lines)
        {
            int size = lines.Length;
            _x = size / 2;
            _y = size / 2;
            _rotateIndex = 0; // look up

            _minX = _minY = 0;
            _maxX = _maxY = size - 1;

            for (var y = 0; y < size; y++)
            {
                var line = lines[y];
                for (var x = 0; x < size; x++)
                {
                    char ch = line[x];
                    if (ch == '#')
                    {
                        Set(x, y, State.Infected);
                    }
                }
            }
        }

        private void Set(int x, int y, State state)
        {
            _infected.Add(Pack(x, y), state);
        }

        /// <summary>
        /// Pack 2D coordinate to int
        /// </summary>
        protected static int Pack(int x, int y) // long would be safer, but no need for the input size
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

        public abstract bool Burst();

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

        protected void Reverse()
        {
            _rotateIndex += 2;
            _rotateIndex %= Dir.Count;
        }

        private void UpdateBounds()
        {
            if (_x > _maxX)
            {
                _maxX = _x;
            }
            else if (_x < _minX)
            {
                _minX = _x;
            }

            if (_y > _maxY)
            {
                _maxY = _y;
            }
            else if (_y < _minY)
            {
                _minY = _y;
            }
        }

        public void Dump()
        {
            for (int j = _minY; j <= _maxY; j++)
            {
                for (int i = _minX; i <= _maxX; i++)
                {
                    if ((_x == i) && (_y == j))
                    {
                        Console.Write('x');
                    }
                    else
                    {
                        if (!_infected.TryGetValue(Pack(i, j), out var state))
                        {
                            state = State.Clean;
                        }

                        char ch;
                        switch (state)
                        {
                            case State.Clean:
                                ch = '.';
                                break;
                            case State.Weakened:
                                ch = 'W';
                                break;
                            case State.Infected:
                                ch = '#';
                                break;
                            case State.Flagged:
                                ch = 'F';
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                        Console.Write(ch);
                    }
                }
                Console.WriteLine();
            }
        }

    }
}