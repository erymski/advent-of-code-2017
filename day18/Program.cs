using System;
using System.IO;

namespace day18
{
    class Program
    {
        //private static readonly string[] test =
        //{
        //    "set a 1",
        //    "add a 2",
        //    "mul a a",
        //    "mod a 5",
        //    "snd a",
        //    "set a 0",
        //    "rcv a",
        //    "jgz a -1",
        //    "set a 1",
        //    "jgz a -2",
        //};

        static void Main(string[] args)
        {
            var engine = new Engine();

            // compile
            var dataPath = Path.Combine(Environment.CurrentDirectory, @"..\..", "input.txt");

            Array.ForEach(File.ReadAllLines(dataPath), line => engine.AddInstruction(line));
            //Array.ForEach(test, line => engine.AddInstruction(line));

            // execute
            engine.Execute();
            Console.WriteLine(engine.LastSound);
            Console.ReadLine();
        }
    }
}
