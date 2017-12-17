using System.Diagnostics;
using System.Text;

namespace day03
{
    [DebuggerDisplay("{" + nameof(Dump) + "}")]
    class Board
    {
        private readonly int _maxDim;
        private readonly int[,] _arr;

        public int Center => _maxDim / 2;

        public Board(int maxDim)
        {
            _maxDim = maxDim;
            _arr = new int[_maxDim, _maxDim];
        }

        public int Total { get; set; }

        public void Assign(int x, int y, int value)
        {
            _arr[x, y] = value;
            Total += value;
        }

        public bool IsGood(int x, int y)
        {
            int count = 0;
            for (int i = x - 1; i <= x + 1; i++)
            {
                for (int j = y - 1; j <= y + 1; j++)
                {
                    if (_arr[i, j] > 0)
                    {
                        count++;
                    }

                    if (count > 1) return true;
                }
            }
            return false;
        }

        public int CalcSum(int x, int y)
        {
            int sum = 0;
            for (int k = x - 1; k <= x + 1; k++)
            {
                for (int m = y - 1; m <= y + 1; m++)
                {
                    sum += _arr[k, m];
                }
            }
            return sum;
        }

        // returns 0 if failed
        public int TryToAssign(int x, int y)
        {
            if (IsGood(x, y))
            {
                int sum = CalcSum(x, y);
                Assign(x, y, sum);
                return sum;
            }
            else
            {
                return 0;
            }
        }

        public string Dump
        {
            get
            {
                StringBuilder builder = new StringBuilder(20 * 1024);
                for (int i = 0; i < _maxDim; i++)
                {
                    for (int j = 0; j < _maxDim; j++)
                    {
                        builder.AppendFormat("{0:D6}", _arr[i, j]);
                        builder.Append(" ");
                    }
                    builder.AppendLine();
                }
                builder.Length--;
                return builder.ToString();
            }
        }
    }
}