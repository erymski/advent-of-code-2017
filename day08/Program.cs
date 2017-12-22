using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day08
{
    class Program
    {
        private static readonly char[] Separator = " ".ToCharArray();

        private static readonly Dictionary<string, Func<int, int, bool>> operators = new Dictionary<string, Func<int, int, bool>> {
            { "==", (a,b) => a == b },
            { "!=", (a,b) => a != b },
            { ">", (a,b) => a > b },
            { "<", (a,b) => a < b },
            { ">=", (a,b) => a >= b },
            { "<=", (a,b) => a <= b }
        };

        enum Command
        {
            inc,
            dec
        }
        class Condition
        {
            public string register;
            public Func<int, int, bool> op;
            public int amount;
        }

        class Instruction
        {
            public string register;
            public Command command;
            public int amount;
            public Condition condition;
        }

        static void Main(string[] args)
        {
            Dictionary<string, int> registers = new Dictionary<string, int>();

            //var input = new[]
            //{
            //    "b inc 5 if a > 1",
            //    "a inc 1 if b < 5",
            //    "c dec -10 if a >= 1",
            //    "c inc -20 if c == 10",
            //};

            var dataPath = Path.Combine(Environment.CurrentDirectory, @"..\..", "input.txt");
            var input = File.ReadAllLines(dataPath);
            var totalMax = int.MinValue;

            foreach (var line in input)
            {
                // parse line
                Instruction instruction = Parse(line);

                // check condition
                int tmp;
                var condition = instruction.condition;
                int condReg = registers.TryGetValue(condition.register, out tmp) ? tmp : 0;
                if (condition.op(condReg, condition.amount))
                {
                    // get current value
                    int value = registers.TryGetValue(instruction.register, out tmp) ? tmp : 0;

                    // change it
                    if (instruction.command == Command.inc)
                    {
                        value += instruction.amount;
                    }
                    else // Command.dec
                    {
                        value -= instruction.amount;
                    }
                    registers[instruction.register] = value;

                    // track for max value of all the time
                    if (value > totalMax)
                    {
                        totalMax = value;
                    }
                }
            }

            int currentMax = registers.Values.Max();

            Console.WriteLine(currentMax);
            Console.WriteLine(totalMax);

            Console.ReadLine();
        }

        // line is "um dec 719 if ms == -1720"
        private static Instruction Parse(string line)
        {
            string[] pieces = line.Split(Separator, StringSplitOptions.RemoveEmptyEntries);
            return new Instruction
            {
                register = pieces[0],
                command = (Command) Enum.Parse(typeof(Command), pieces[1], true),
                amount = int.Parse(pieces[2]),
                condition = new Condition
                {
                    register = pieces[4],
                    op = operators[pieces[5]],
                    amount = int.Parse(pieces[6])
                }
            };
        }
    }
}
