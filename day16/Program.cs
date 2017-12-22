using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day16
{
    class Program
    {
        private static readonly char[] Separator = "/".ToCharArray();

        enum CommandType
        {
            Spin,
            SwapPos,
            SwapName
        }

        // don't bother with inheritance
        struct Cmd
        {
            public CommandType type;
            public int size;
            public int pos1;
            public int pos2;
            public char ch1;
            public char ch2;
        }


        static void Main()
        {
            List<char> arr = new List<char>(Enumerable.Range(0, 16).Select(ch => (char)('a' + ch)));

            //List<char> arr = new List<char>("abcdefghijklmnop".ToCharArray());
            //List<char> arr = new List<char>("abcde".ToCharArray());
            int length = arr.Count;

            //string[] commands = { "s1", "x3/4", "pe/b" };
            var dataPath = Path.Combine(Environment.CurrentDirectory, @"..\..", "input.txt");
            string[] commands = File.ReadAllText(dataPath).Split(",".ToCharArray());
            List<Cmd> cmds = new List<Cmd>(commands.Length);

            foreach (var command in commands)
            {
                switch (command[0])
                {
                    case 's': // spin
                        var size = int.Parse(command.Substring(1));
                        cmds.Add(new Cmd { type = CommandType.Spin, size = size });
                        break;
                    case 'x': // swap positions
                        var posArray = command.Substring(1).Split(Separator).Select(int.Parse).ToArray();
                        cmds.Add(new Cmd { type = CommandType.SwapPos, pos1 = posArray[0], pos2 = posArray[1] });
                        break;
                    case 'p': // swap names
                        cmds.Add(new Cmd { type = CommandType.SwapName, ch1 = command[1], ch2 = command[3]});
                        break;
                }
            }

            var res1 = Run(new List<char>(arr), cmds, 1);
            Console.WriteLine(res1);

            var res2 = Run(new List<char>(arr), cmds, 1000000000);
            Console.WriteLine(res2);
            Console.ReadKey();
        }

        private static string Run(List<char> arr, IReadOnlyCollection<Cmd> cmds, int iterations)
        {
            int length = arr.Count;
            var cache = new Dictionary<string, string>();

            string before = string.Join(string.Empty, arr);
            for (int i = 0; i < iterations; i++)
            {
                // see if the input is already processed
                string after;
                if (!cache.TryGetValue(before, out after))
                {
                    foreach (var command in cmds)
                    {
                        switch (command.type)
                        {
                            case CommandType.Spin: // spin
                                arr.InsertRange(0, arr.GetRange(length - command.size, command.size));
                                arr.RemoveRange(length, command.size);
                                break;
                            case CommandType.SwapPos: // swap positions
                                var tmp = arr[command.pos1];
                                arr[command.pos1] = arr[command.pos2];
                                arr[command.pos2] = tmp;
                                break;
                            case CommandType.SwapName: // swap names
                                int firstPos = arr.IndexOf(command.ch1);
                                int secondPos = arr.IndexOf(command.ch2);

                                arr[firstPos] = command.ch2;
                                arr[secondPos] = command.ch1;
                                break;
                        }
                    }

                    after = string.Join(string.Empty, arr);

                    cache.Add(before, after);
                }

                // TODO: further optimization - just take index when loop detected and calc what index would be for billionth iteration

                before = after;
            }
            return before;
        }
    }
}
