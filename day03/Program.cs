using System;

namespace day03
{
    struct Dir
    {
        public int dx;
        public int dy;
    }

    class Program
    {
        /// <summary>
        /// Directions for CCW walk: up, left, down, right.
        /// </summary>
        static readonly Dir[] dirs =
        {
            new Dir { dx = 0, dy = 1 },
            new Dir { dx = -1, dy = 0 },
            new Dir { dx = 0, dy = -1 },
            new Dir { dx = 1, dy = 0 },
        };

        static void Main(string[] args)
        {
            //int pos = FillAndFind(125);
            int pos = FillAndFind(277678);

            //var pathLength1 = Calc(1);
            //var pathLength2 = Calc(12);
            //var pathLength3 = Calc(23);
            //var pathLength4 = Calc(1024);
            var pathLength = Calc(277678);
        }

        /// <summary>
        /// Empirical way to solve Task #1.
        /// Was not useful to solve Task #2, but anyway.
        /// </summary>
        private static int Calc(int square)
        {
            if (square == 1) return 0;

            // find dimension of "box" where the square lives
            int dimension = 1;
            int currMax;
            while (true)
            {
                currMax = dimension * dimension;
                if (currMax > square) break;

                // side of each new layer is bigger that prev one by 2
                dimension += 2;
            }

            // find side of the box where the square lives
            int prevCorner = 0;
            for (int j = 0; j < 4; j++)
            {
                prevCorner = currMax - dimension + 1;
                if (square >= prevCorner) break;

                currMax = prevCorner;
            }

            var half = dimension / 2; // that's number of steps from center to boundary

            // assume that now we at middle square of the side
            var middleSquare = prevCorner + half;
            var distanceFromMiddle = Math.Abs(middleSquare - square);
            return /*from center to boundary*/ half + /*from border center to the square */ distanceFromMiddle;
        }

        private static int FillAndFind(int toFind)
        {
            var board = new Board(21);
            int center = board.Center;
            int i = center, j = center;

            // set center
            board.Assign(center, center, 1);

            // set next point
            int sum = board.CalcSum(++i, j);
            board.Assign(i, j, sum);

            int dirIndex = 0;
            var dir = dirs[dirIndex];

            while (true)
            {
                var value = board.TryToAssign(i + dir.dx, j + dir.dy);
                if (value > 0)
                {
                    if (value > toFind) return value;

                    i += dir.dx;
                    j += dir.dy;
                }
                else
                {
                    // time to change direction
                    dirIndex = (dirIndex + 1) % 4;
                    dir = dirs[dirIndex];
                }
            }
        }
    }
}
