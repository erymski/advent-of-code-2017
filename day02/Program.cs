using System;
using System.IO;
using System.Linq;

namespace day02
{
    class Program
    {
        //private static readonly string[] testInput1 = {"5,1,9,5", "7,5,3", "2,4,6,8"};
        //private static readonly string[] testInput2 = {"5,9,2,8", "9,4,7,3", "3,8,6,5"};
        private static readonly char[] Separator = ",".ToCharArray();

        static void Main(string[] args)
        {
//            int sum1 = GetChecksum(testInput);

            var dataPath = Path.Combine(Environment.CurrentDirectory, @"..\..", "input.txt");
            var data = File.ReadAllLines(dataPath);
            var sum = GetChecksum(data);
            Console.WriteLine(sum);

            Console.ReadLine();
        }

        private static int GetChecksum(string[] lines)
        {
            int sum = 0;
            foreach (var line in lines)
            {
                var numbers = line.Split(Separator).Select(int.Parse).ToArray();
                sum += LineDivide(numbers); // task 2
                //sum += LineChecksum(numbers); // task 1
            }
            return sum;
        }

        private static int LineChecksum(int[] numbers)
        {
            int min = numbers[0];
            int max = numbers[0];
            for (int i = 1; i < numbers.Length; i++)
            {
                var curr = numbers[i];
                if (curr > max)
                {
                    max = curr;
                }
                else if (curr < min)
                {
                    min = curr;
                }
            }
            return max - min;
        }

        private static int LineDivide(int[] numbers)
        {
            var length = numbers.Length;

            for (int i = 0; i < length; i++)
            {
                var a = numbers[i];
                for (int j = i + 1; j < length; j++)
                {
                    var b = numbers[j];
                    if (a > b)
                    {
                        if (a % b == 0)
                        {
                            return a / b;
                        }
                    }
                    else
                    {
                        if (b % a == 0)
                        {
                            return b / a;
                        }
                    }
                }
            }
            throw new Exception("Something wrong");
        }
    }
}
