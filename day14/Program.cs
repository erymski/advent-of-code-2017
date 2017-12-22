using System;
using System.Linq;
using System.Text;

namespace day14
{
    class Program
    {
        static void Main()
        {
            // prepare data
            char[][] arr = new char[128][];
            for (int i = 0; i <= 127; i++)
            {
                var bytes = Reorder("vbqugkhl-" + i);
                arr[i] = ToStr(bytes).ToCharArray();
            }

            // clear regions one by one
            int regions = 0;
            int cells = 0;
            while (true)
            {
                bool found = false;

                for (int rowIndex = 0; rowIndex < 128; rowIndex++) // place to optimize... no need to start from 0 each time
                {
                    var row = arr[rowIndex];
                    var column = Array.IndexOf(row, '1');
                    if (column == -1) continue;

                    regions++;

                    int total = RemoveRegion(arr, rowIndex, column);
                    cells += total;

                    found = true;
                    break;
                }

                // check if no regions found
                if (!found) break;
            }

            Console.WriteLine(cells);
            Console.WriteLine(regions);
            Console.ReadLine();
        }

        /// <summary>
        /// Remove cell and its region
        /// </summary>
        /// <returns>Number of cleaned cells.</returns>
        private static int RemoveRegion(char[][] arr, int row, int column)
        {
            if (row > 127 || column > 127 || row < 0 || column < 0) return 0;

            // check if the cell needs to be cleaned
            if (arr[row][column] == '1')
            {
                arr[row][column] = '0';

                // recursively clear connected cells
                return 1 + RemoveRegion(arr, row - 1, column) + RemoveRegion(arr, row, column - 1) + RemoveRegion(arr, row, column + 1) + RemoveRegion(arr, row + 1, column);
            }
            return 0;
        }

        private static byte[] Reorder(string str)
        {
            byte[] range = Enumerable.Range(0, 256).Select(ch => (byte)ch).ToArray();

            // add standard suffix
            var input = str.Select(ch => (byte)ch).Concat(new byte[] { 17, 31, 73, 47, 23 }).ToArray();

            int length = range.Length;
            int skip = 0;
            int startPos = 0;

            for (int i = 0; i < 64; i++)
            {
                foreach (var selectionLength in input)
                {
                    int endPos = startPos + selectionLength - 1;
                    int tmpStart = startPos;

                    // reverse array
                    while (endPos > tmpStart)
                    {
                        int normalizedEndPos = endPos % length;
                        int normalizedStartPos = tmpStart % length;

                        // swap
                        byte tmp = range[normalizedStartPos];
                        range[normalizedStartPos] = range[normalizedEndPos];
                        range[normalizedEndPos] = tmp;

                        endPos--;
                        tmpStart++;
                    }

                    startPos += selectionLength;
                    startPos += skip;
                    startPos %= length;
                    skip++;
                }
            }

            return range;
        }

        private static string ToStr(byte[] hashed)
        {
            var builder = new StringBuilder();
            byte acc = 0;
            bool firstRound = true;
            int count = 0;
            foreach (byte b in hashed)
            {
                if (firstRound)
                {
                    acc = b;
                    firstRound = false;
                }
                else
                {
                    acc = (byte)(acc ^ b);
                }

                count++;
                if (count % 16 == 0)
                {
                    firstRound = true;
                    builder.AppendFormat(Convert.ToString(acc, 2).PadLeft(8, '0'));
                }
            }

            return builder.ToString();
        }
    }
}
