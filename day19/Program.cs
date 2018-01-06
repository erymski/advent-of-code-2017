using System;
using System.IO;

namespace day19
{
    class Program
    {
        //private static readonly string[] rows = {
        //                                        "     |         ",
        //                                        "     |  +--+   ",
        //                                        "     A  |  C   ",
        //                                        " F---|----E|--+",
        //                                        "     |  |  |  D",
        //                                        "     +B-+  +--+"
        //                                    };

        private const char EmptySpace = ' ';
        private const char HorizontalStep = '-';
        private const char VerticalStep = '|';
        private const char Turn = '+';

        static void Main()
        {
            var dataPath = Path.Combine(Environment.CurrentDirectory, @"..\..", "input.txt");
            var rows = File.ReadAllLines(dataPath);

            var rowCount = rows.Length;
            var columnCount = rows[0].Length;

            // utility func to check if position is within input
            Func<Dir, bool> isValid = d => d.y >= 0 &&
                                              d.y < rowCount &&
                                              d.x >= 0 &&
                                              d.x < columnCount;

            // utility func to get char under position
            Func<Dir, char> getChar = d => rows[d.y][d.x];


            // start position is first row (from top) and the only cell, with '|' in it.
            var pos = new Dir { x = rows[0].IndexOf(VerticalStep), y = 0 };

            // Vector with direction. Initial direction is go down
            var dir = new Dir { x = 0, y = 1 };


            int steps = 0;
            while (true)
            {
                steps++;

                pos = pos.Add(dir);
                char currChar = getChar(pos);

                // check if it's end of the path
                if (currChar == EmptySpace) break;

                // just follow path
                if (currChar == HorizontalStep || currChar == VerticalStep) continue;

                // time to change dir
                if (currChar == Turn)
                {
                    // find turn direction
                    foreach (var sideDir in dir.Sides())
                    {
                        Dir candidatePos = pos.Add(sideDir);
                        if (! isValid(candidatePos)) continue;

                        char candidateChar = getChar(candidatePos);
                        if (candidateChar == EmptySpace) continue;

                        // candidate cell contains the further trail, so change direction
                        dir = sideDir;
                        break;
                    }
                    continue;
                }

                // unchecked chars are letters. Dump it
                Console.Write(currChar);
            }

            Console.WriteLine();
            Console.WriteLine(steps);

            Console.ReadLine();
        }
    }
}
