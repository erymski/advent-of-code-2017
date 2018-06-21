using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using MathNet.Numerics.LinearAlgebra;

namespace day21
{
    /// <summary>
    /// Square matrix.
    /// </summary>
    [DebuggerDisplay("{" + nameof(DebugView) + "}")]
    class Square
    {
        public Matrix<float> Matrix { get; }
        public int Size => Matrix.RowCount;

        public int PixelCount { get; }

        public Square(Matrix<float> matrix)
        {
            Matrix = matrix;
            PixelCount = (int) Math.Round(matrix.Enumerate().Sum());
        }

        internal static Square Process(string input)
        {
            var rows = input.Split('/');
            int size = rows[0].Length;
            var matrix = Matrix<float>.Build.Dense(size, size);
            for (var i = 0; i < size; i++)
            {
                for (var j = 0; j < size; j++)
                {
                    if (rows[i][j] == '#')
                    {
                        matrix.At(j, i, 1);
                    }
                }
            }

            return new Square(matrix);
        }

        public IEnumerable<Matrix<float>> Alike()
        {
            foreach (var matrix in AllRotations(Matrix)) yield return matrix;
            foreach (var matrix in AllRotations(Matrix.Transpose())) yield return matrix;
        }

        private IEnumerable<Matrix<float>> AllRotations(Matrix<float> input)
        {
            yield return input;

            Matrix<float> ccw1 = Rotate(input);
            yield return ccw1;

            Matrix<float> ccw2 = Rotate(ccw1);
            yield return ccw2;

            Matrix<float> ccw3 = Rotate(ccw2);
            yield return ccw3;
        }

        private Matrix<float> Rotate(Matrix<float> input)
        {
            return ReverseRows(input.Transpose());
        }

        private Matrix<float> ReverseRows(Matrix<float> input)
        {
            for (int i = 0; i < input.RowCount; i++)
            {
                var row = input.Row(i).ToArray();
                Array.Reverse(row);
                input.SetRow(i, row);
            }

            return input;
        }

        public string DebugView
        {
            get
            {
                StringBuilder builder = new StringBuilder(50);
                int i = 0;
                foreach (float value in Matrix.Enumerate())
                {
                    builder.Append(value > 0 ? '#' : '.');
                    i++;
                    if (i % Size == 0)
                    {
                        builder.AppendLine();
                    }
                }

                builder.Length--;
                return builder.ToString();
            }
        }
    }

    class Rule
    {
        private readonly Square _from;

        public int Size => _from.Size;

        public Square To { get; }

        public Rule(string line)
        {
            var pair = line.Split(new[] {" => "}, 2, StringSplitOptions.None);
            _from = Square.Process(pair[0]);
            To = Square.Process(pair[1]);
        }

        public bool Match(Square piece)
        {
            if (piece.PixelCount == _from.PixelCount)
            {
                foreach (var matrix in _from.Alike())
                {
                    if (matrix.Equals(piece.Matrix)) return true;
                }
            }

            return false;
        }
    }

    class Program
    {
        private const string start = ".#./..#/###";

        static void Main()
        {
            Stopwatch watch = Stopwatch.StartNew();

            var dataPath = Path.Combine(Environment.CurrentDirectory, @"..\..", "input.txt");
            var lines = File.ReadAllLines(dataPath);
            var (rules2, rules3) = ToRules(lines);

            var square = Square.Process(start);
            for (int i = 0; i < 18; i++)
            {
                square = Transform(square, square.Size % 2 == 0 ? rules2 : rules3);
            }

            watch.Stop();
            Console.WriteLine($"Pixels count: {square.PixelCount} (took {watch.ElapsedMilliseconds}ms)");
            Console.ReadKey();
        }

        private static Square Transform(Square square, Rule[] rules)
        {
            int chunkSize = rules[0].Size;
            var newChunkSize = chunkSize + 1;

            int chunks = square.Size / chunkSize;
            Matrix<float> output = Matrix<float>.Build.Dense(chunks * newChunkSize, chunks * newChunkSize);
            for (int i = 0; i < chunks; i++)
            {
                for (int j = 0; j < chunks; j++)
                {
                    Matrix<float> piece = square.Matrix.SubMatrix(i * chunkSize, chunkSize, j * chunkSize, chunkSize);
                    var sqPiece = new Square(piece);
                    bool matched = false;
                    foreach (var rule in rules)
                    {
                        if (rule.Match(sqPiece))
                        {
                            output.SetSubMatrix(i * newChunkSize, j * newChunkSize, rule.To.Matrix);
                            matched = true;
                            break;
                        }
                    }

                    if (!matched)
                    {
                        throw new Exception("No matches!");
                    }
                }
            }

            return new Square(output);
        }

        private static (Rule[], Rule[]) ToRules(string[] lines)
        {
            var groups = lines.Select(line => new Rule(line)).GroupBy(rule => rule.Size == 2).ToArray();
            return (groups[0].ToArray(), groups[1].ToArray());
        }
    }
}
