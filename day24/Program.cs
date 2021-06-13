using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day24
{
    class Component
    {
        private readonly int _first;
        private readonly int _second;

        private Component(int first, int second)
        {
            _first = first;
            _second = second;
        }

        public bool Has(int x)
        {
            return (x == _first) || (x == _second);
        }

        public int Other(int x)
        {
            return x == _first ? _second : _first;
        }

        public int Strength => _first + _second;

        internal static Component Parse(string input)
        {
            int[] pair = input.Split("/".ToCharArray(), StringSplitOptions.None)
                .Select(int.Parse)
                .ToArray();

            return new Component(pair[0], pair[1]);
        }
    }

    class BridgeBuilder
    {
        class Branch
        {
            public Component head;
            public ICollection<Component> rest;
        }

        private int _longestLength = int.MinValue;
        private int _longestStrength = int.MinValue;

        public void Process(Component[] components)
        {
            int max = Split(components, 0)
                .Max(b => ProcessBranch(b, 0, sum: 0, length: 1));

            Console.WriteLine($"Max strength = {max}");
            Console.WriteLine($"Longest bridge: length = {_longestLength}; strength = {_longestStrength}");
        }

        private static ICollection<Branch> Split(ICollection<Component> sequence, int splitBy)
        {
            return sequence
                .Where(c => c.Has(splitBy))
                .Select(c => new Branch
                {
                    head = c,
                    rest = sequence.Except(new[] { c }).ToList()
                })
                .ToList();
        }

        private int ProcessBranch(Branch branch, int startWith, int sum, int length)
        {
            int next = branch.head.Other(startWith);
            var newSum = sum + branch.head.Strength;

            ICollection<Branch> further = Split(branch.rest, next);
            if (further.Count == 0)
            {
                if (_longestLength < length)
                {
                    _longestLength = length;
                    _longestStrength = newSum;
                }
                else if (_longestLength == length)
                {
                    _longestStrength = Math.Max(_longestStrength, newSum);
                }

                return newSum;
            }

            return further.Max(b => ProcessBranch(b, next, newSum, length + 1));
        }
    }

    class Program
    {
        static void Main()
        {
            var dataPath = Path.Combine(Environment.CurrentDirectory, @"..\..", "input.txt");
            var lines = File.ReadAllLines(dataPath);
            var components = Array.ConvertAll(lines, Component.Parse);

            var builder = new BridgeBuilder();
            builder.Process(components);

            Console.ReadLine();
        }

    }
}
