using System;
using System.IO;

namespace day09
{
    class Program
    {
        static void Main()
        {
            int garbage = 0;

            //int count = CountGroups("{}");
            //int count2 = CountGroups("{{{}}}");
            //int count3 = CountGroups("{{},{}}");
            //int count4 = CountGroups("{{{},{},{{}}}}");
            //int count5 = CountGroups1("{<a>,<a>,<a>,<a>}", ref garbage);
            //int count6 = CountGroups1("{{<ab>},{<ab>},{<ab>},{<ab>}}", ref garbage);
            //int count7 = CountGroups1("{{<!!>},{<!!>},{<!!>},{<!!>}}", ref garbage);
            //int count8 = CountGroups("{{<a!>},{<a!>},{<a!>},{<ab>}}");

            var dataPath = Path.Combine(Environment.CurrentDirectory, @"..\..", "input.txt");
            var input = File.ReadAllText(dataPath);

            int res1 = CountGroups1(input, ref garbage);
            Console.WriteLine(res1);
            Console.WriteLine(garbage);

            Console.ReadLine();
        }

        private static int CountGroups1(string input, ref int garbage)
        {
            garbage = 0;
            int groupCompleted = 0;
            int nested = 0;
            bool skipNext = false;
            bool insideGarbage = false;

            foreach (char ch in input)
            {
                if (skipNext)
                {
                    skipNext = false;
                    continue;
                }

                if (ch == '!') // ignore next char
                {
                    skipNext = true;
                    continue;
                }

                if (insideGarbage)
                {
                    if (ch == '>') // end of garbage
                    {
                        insideGarbage = false;
                    }
                    else
                    {
                        garbage++;
                    }
                    continue;
                }

                if (ch == '<') // garbage started
                {
                    insideGarbage = true;
                    continue;
                }

                if (ch == '{') // group started
                {
                    nested++;
                }
                else if (ch == '}') // group completed
                {
                    groupCompleted += nested;
                    nested--;
                }
            }

            return groupCompleted;
        }
//        private static int CountGroups(string input)
//        {
//            int groupStarted = 0;
//            int groupCompleted = 0;
//            int nested = 0;
//            bool skipNext = false;
//            bool insideGarbage = false;

//            foreach (char ch in input)
//            {
//                if (skipNext)
//                {
//                    skipNext = false;
//                    continue;
//                }

//                if (ch == '!')
//                {
//                    skipNext = true;
//                    continue;
//                }

//                if (insideGarbage)
//                {
//                    if (ch == '>')
//                    {
//                        insideGarbage = false;
//                    }
//                    continue;
//                }

//                if (ch == '<')
//                {
//                    insideGarbage = true;
//                    continue;
//                }

//                if (ch == '{')
//                {
//                    nested++;
////                    groupStarted++;
//                }
//                else if (ch == '}')
//                {
//                    groupCompleted += nested;
//                    nested--;
//                }
//            }

//            return groupCompleted;
//        }
    }
}
