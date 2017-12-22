using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace day10
{
    class Program
    {
        static void Main()
        {
            var inputString = "31,2,85,1,80,109,35,63,98,255,0,13,105,254,128,33";

            byte[] officialInput = inputString.Split(",".ToCharArray()).Select(byte.Parse).ToArray();
            var res = Reorder(officialInput);
            Console.WriteLine(res[0] * res[1]);

            byte[] input = inputString.Select(ch => (byte)ch).ToArray();
            byte[] hashed = KnotHash(input);

            string hashString = ToStr(hashed);
            Console.WriteLine(hashString);
            Console.ReadLine();
        }

        private static string ToStr(byte[] hashed)
        {
            StringBuilder builder = new StringBuilder();
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
                    builder.AppendFormat(acc.ToString("x2"));
                }
            }

            return builder.ToString();
        }

        private static byte[] KnotHash(IEnumerable<byte> input)
        {
            // init buffer with 0..255 values
            byte[] range = Enumerable.Range(0, 256).Select(ch => (byte)ch).ToArray();

            // add standard suffix
            var bytes = input.Concat(new byte[] {17, 31, 73, 47, 23}).ToArray();

            int length = range.Length;
            int skip = 0;
            int startPos = 0;

            for (int i = 0; i < 64; i++)
            {
                foreach (var selectionLength in bytes)
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

        private static byte[] Reorder(byte[] input)
        {
            // init buffer with 0..255 values
            byte[] range = Enumerable.Range(0, 256).Select(ch => (byte)ch).ToArray();

            int length = range.Length;
            int skip = 0;
            int startPos = 0;

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

            return range;
        }
    }
}
