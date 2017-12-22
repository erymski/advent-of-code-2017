using System;

namespace day15
{
    class Program
    {
        static void Main(string[] args)
        {
            ulong genA = 116; //65;
            ulong genB = 299; //8921;

            var res1 = Task1(genA, genB);
            Console.WriteLine(res1);

            var res2 = Task2(genA, genB);
            Console.WriteLine(res2);

            Console.ReadLine();
        }

        private static int Task1(ulong genA, ulong genB)
        {
            int judge = 0;

            for (int i = 0; i < 40000000; i++)
            {
                genA = (genA * 16807) % 2147483647;
                genB = (genB * 48271) % 2147483647;

                if ((genA & 0xffff) == (genB & 0xffff))
                {
                    judge++;
                }
            }

            return judge;
        }

        private static int Task2(ulong genA, ulong genB)
        {
            int judge = 0;

            for (int i = 0; i < 5000000; i++)
            {
                do
                {
                    genA = (genA * 16807) % 2147483647;
                } while (genA % 4 != 0);

                do
                {
                    genB = (genB * 48271) % 2147483647;
                } while (genB % 8 != 0);

                if ((genA & 0xffff) == (genB & 0xffff))
                {
                    judge++;
                }
            }

            return judge;
        }
    }
}
