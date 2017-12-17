using System;
using System.Collections.Generic;

namespace day17
{
    class Program
    {
        private const int input = 356;

        static void Main(string[] args)
        {
            int after2017 = TaskOne();
            Console.WriteLine(after2017);

            int afterZero = TaskTwo();
            Console.WriteLine(afterZero);

            Console.ReadKey();
        }

        /// <summary>
        /// Find next number after 2017.
        /// </summary>
        private static int TaskOne()
        {
            int pos = 0;
            List<int> buffer = new List<int>(2100) { 0 };

            for (int i = 1; i < 2018; i++)
            {
                // shift pos
                pos = (pos + input) % i;
                pos++;

                buffer.Insert(pos, i);
            }

            // return the next number after the last inserted
            return buffer[(pos + 1) % buffer.Count];
        }

        /// <summary>
        /// Find next number after zero.
        /// </summary>
        private static int TaskTwo()
        {
            int pos = 0; // current position
            int last = 0;

            for (int i = 1; i < 50000000; i++)
            {
                // shift pos
                pos = (pos + input) % i;
                pos++;

                // zero is at the first position, so track what number was assigned last to the second pos
                if (pos == 1)
                {
                    last = i;
                }
            }
            return last;
        }
    }
}
