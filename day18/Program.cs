using System;
using System.IO;

namespace day18
{
    class Program
    {
        //private static readonly string[] lines =
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
            var dataPath = Path.Combine(Environment.CurrentDirectory, @"..\..", "input.txt");
            var lines = File.ReadAllLines(dataPath);

            //var lastSound = Task1(lines);
            //Console.WriteLine(lastSound);

            var sndCount = Task2(lines);
            Console.WriteLine(sndCount);

            Console.ReadLine();
        }

        private static long Task1(string[] lines)
        {
            var engine = new Engine();

            // compile
            Array.ForEach(lines, line => engine.AddInstruction(line));

            // execute
            engine.Execute();

            var lastSound = engine.LastSound;
            return lastSound;
        }

        private static long Task2(string[] lines)
        {
            //lines = new[]
            //{
            //    "snd 1",
            //    "snd 2",
            //    "snd p",
            //    "rcv a",
            //    "rcv b",
            //    "rcv c",
            //    "rcv d",
            //};

            return Duet.Play(lines);
        }
    }
}