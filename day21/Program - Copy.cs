using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;

namespace day21
{
    class Square
    {
        private readonly Matrix<byte> _matrix;

        internal int Size => _matrix.RowCount;

        public Square(string input)
        {
            _matrix = ToMatrix(input);
        }

        internal static Matrix<byte> ToMatrix(string input)
        {
            int size = input.Length;
            var matrix = Matrix<byte>.Build.Dense(size, size);

            var rows = input.Split('/');
            for (var i = 0; i < size; i++)
            {
                for (var j = 0; j < size; j++)
                {
                    if (rows[i][j] == '#')
                    {
                        matrix.At(i, j, 1);
                    }
                }
            }

            return matrix;
        }
    }

    class Rule
    {
        private Matrix<byte> _to;
        private Matrix<byte> _from;

        public int Size { get; }

        public Rule(string line)
        {
            var pair = line.Split(new[] {" => "}, StringSplitOptions.None);
            Size = pair[0].Length;
            _from = Square.ToMatrix(pair[0]);
//            _matches = ToCombinations(from).ToArray();

            _to = Square.ToMatrix(pair[1]);
        }

        //private IEnumerable<int> ToCombinations(string[] square)
        //{
        //    // original
        //    yield return ToHash(square);

        //    // Y flip
        //    var flipY = square.Reverse().ToArray();
        //    yield return ToHash(flipY);

        //    // X flip
        //    yield return ToHash(square.Select(s => new string(s.Reverse().ToArray())));
        //}

        private static int ToHash(IEnumerable<string> rows)
        {
            string whole = string.Join(string.Empty, rows);

            int result = 0;
            foreach (char c in whole)
            {
                if (c == '#')
                {
                    result += 1;
                }

                result <<= 1;
            }

            return result;
        }
    }

    class Program
    {
        private const string start = ".#./..#/###";

        static void Main()
        {
            var dataPath = Path.Combine(Environment.CurrentDirectory, @"..\..", "input.txt");
            var lines = File.ReadAllLines(dataPath);
            var (rules2, rules3) = ToRules(lines);

            var square = new Square(start);
            for (int i = 0; i < 5; i++)
            {
                square = Transform(square, square.Size % 2 == 0 ? rules2 : rules3);
            }
        }

        private static Square Transform(Square square, Rule[] rules)
        {
            
        }

        private static (Rule[], Rule[]) ToRules(string[] lines)
        {
            var groups = lines.Select(line => new Rule(line)).GroupBy(rule => rule.Size == 2).ToArray();
            return (groups[0].ToArray(), groups[1].ToArray());
        }
    }
}
