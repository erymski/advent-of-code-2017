using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day23
{
    class Program
    {
        static readonly Dictionary<char, int> _regs = new Dictionary<char, int>();
        private static int _mulCount = 0;

        static void Main(string[] args)
        {
            var dataPath = Path.Combine(Environment.CurrentDirectory, @"..\..", "input.txt");
            var lines = File.ReadAllLines(dataPath)/*_test*/;
            var actions = Array.ConvertAll(lines, ToAction);
            foreach (var i in Enumerable.Range(0, 8))
            {
                char ch = Convert.ToChar(Convert.ToByte('a') + (byte)i);
                _regs[ch] = 0;
            }

            int pos = 0;
            while (true)
            {
                int offset = actions[pos]();
                pos += offset;
                if (pos < 0 || pos >= actions.Length) break;
            }

            Console.WriteLine($"Done: {_mulCount}");
            Console.ReadKey();
        }

        private static Func<int> ToAction(string line)
        {
            // all instructions are like "mul X Y"
            var pieces = line.Split(' ');

            var cmd = pieces[0]; // three chars

            string regString = pieces[1]; // string with reg or number
            char reg = regString[0];

            var value = pieces[2]; // string with reg or number

            Func<int> action;
            switch (cmd)
            {
                case "set":
                    action = () =>
                    {
                        _regs[reg] = AsValue(value);
                        return 1;
                    };
                    break;

                case "mul":
                    action = () =>
                    {
                        _mulCount++;
                        _regs[reg] *= AsValue(value);
                        return 1;
                    };
                    break;

                case "sub":
                    action = () =>
                    {
                        _regs[reg] -= AsValue(value);
                        return 1;
                    };
                    break;

                case "jnz":
                    action = () => { return AsValue(regString) == 0 ? 1 : AsValue(value); };
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            return action;
        }

        private static int AsValue(string value)
        {
            return char.IsLetter(value[0]) ? _regs[value[0]] : int.Parse(value);
        }
    }
}
