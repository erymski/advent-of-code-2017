using System;
using System.Collections.Generic;
using System.Text;

namespace day06
{
    class Program
    {
        static void Main(string[] args)
        {
            //var input = new [] { 0, 2, 7, 0 };
            var input = new [] { 2, 8, 8, 5, 4, 2, 3, 1, 5, 5, 1, 2, 15, 13, 5, 14 };

            var cycles1 = Task1((int[])input.Clone());
            Console.WriteLine(cycles1);

            var cycles2 = Task2((int[])input.Clone());
            Console.WriteLine(cycles2);
            Console.ReadLine();
        }

        private static int Task1(int[] input)
        {
            int length = input.Length;
            var set = new HashSet<string>(); // can be faster with numeric hash, NBD for small dataset

            bool firstTime = true;
            int iteration = 0;
            while (true)
            {
                // find index of max
                int index = 0;
                for (int i = 1; i < length; i++)
                {
                    if (input[index] < input[i])
                    {
                        index = i;
                    }
                }

                // reset max cell
                int amount = input[index];
                input[index] = 0;

                // distribute among the other cells
                while (amount > 0)
                {
                    index = (index + 1) % length;
                    input[index]++;
                    amount--;
                }
                iteration++;

                var str = ToStr(input);
                if (set.Contains(str))
                {
                    return iteration;
                }
                set.Add(str);
            }
        }

        private static int Task2(int[] input)
        {
            int length = input.Length;
            var set = new HashSet<string>();

            bool firstPhase = true;
            int iteration = 0;
            string lookingFor = null;
            while (true)
            {
                // find index of max
                int index = 0;
                for (int i = 1; i < length; i++)
                {
                    if (input[index] < input[i])
                    {
                        index = i;
                    }
                }

                // reset max cell
                int amount = input[index];
                input[index] = 0;

                // distribute among the other cells
                while (amount > 0)
                {
                    index = (index + 1) % length;
                    input[index]++;
                    amount--;
                }

                var str = ToStr(input);
                if (firstPhase)
                {
                    if (set.Contains(str))
                    {
                        firstPhase = false;
                    }
                    else
                    {
                        set.Add(str);
                    }
                }
                else
                {
                    if (lookingFor == null)
                    {
                        lookingFor = str;
                    }
                    else
                    {
                        if (str == lookingFor) break;
                    }

                    iteration++;
                }
            }
            return iteration;
        }

        private static string ToStr(int[] array)
        {
            StringBuilder builder = new StringBuilder(20*1024);
            for (int i = 0; i < array.Length; i++)
            {
                builder.Append(array[i].ToString("D5"));
            }

            return builder.ToString();
        }
    }
}
