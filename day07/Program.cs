using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace day07
{
    class Program
    {
        private static readonly string[] ChildSeparator = {"->"};
        private static readonly char[] ParentCleanup = {'(', ')', ' '};
        private static readonly char[] DepsCleanup = { ',', ' ' };

        class Node
        {
            public string Name;
            public int Weight;
            public readonly List<Node> Children = new List<Node>();

            public int TotalWeigth
            {
                get { return Weight + Children.Sum(n => n.TotalWeigth); }
            }
        }

        static void Main(string[] args)
        {
            //var input = new[]
            //{
            //    "pbga(66)",
            //    "xhth(57)",
            //    "ebii(61)",
            //    "havc(66)",
            //    "ktlj(57)",
            //    "fwft(72)->ktlj, cntj, xhth",
            //    "qoyq(66)",
            //    "padx(45)->pbga, havc, qoyq",
            //    "tknk(41)->ugml, padx, fwft",
            //    "jptl(61)",
            //    "ugml(68)->gyxo, ebii, jptl",
            //    "gyxo(61)",
            //    "cntj(57)"
            //};

            var dataPath = Path.Combine(Environment.CurrentDirectory, @"..\..", "input.txt");
            string[] input = File.ReadAllLines(dataPath);


            var res1 = Task1(input);
            Console.WriteLine(res1);

            var res2 = Task2(input);
            Console.WriteLine(res2);

            Console.ReadKey();
        }

        private static string Task2(string[] input)
        {
            var nodes = new Dictionary<string, Node>(); // name -> node
            var relations = new Dictionary<string, string>(); // child -> parent

            // parse input
            foreach (var line in input)
            {
                var pieces = line.Split(ChildSeparator, StringSplitOptions.RemoveEmptyEntries);

                var parentNode = pieces[0];
                var nameAndWeigth = parentNode.Split(ParentCleanup, StringSplitOptions.RemoveEmptyEntries);
                var node = new Node {Name = nameAndWeigth[0], Weight = int.Parse(nameAndWeigth[1])};

                nodes.Add(node.Name, node);

                if (pieces.Length > 1)
                {
                    var children = pieces[1];
                    var dependencies = children.Split(DepsCleanup, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var depId in dependencies)
                    {
                        // store relation to parent
                        relations.Add(depId, node.Name);
                    }
                }
            }

            // build the tree
            //  go through child->parent table and fill up 'childen' field for node
            foreach (KeyValuePair<string, string> childParent in relations)
            {
                nodes[childParent.Value].Children.Add(nodes[childParent.Key]);
            }

            // detect top node
            //  run again and remove all nodes, who has parent
            foreach (KeyValuePair<string, string> childParent in relations)
            {
                nodes.Remove(childParent.Key);
            }

            // the only node without parent is the top node
            var topNode = nodes.First();
            return topNode.Key;
        }

        private static string Task1(string[] input)
        {
            var ids = new HashSet<string>();
            var children = new HashSet<string>();

            foreach (var line in input)
            {
                var pieces = line.Split(new[] { "->" }, StringSplitOptions.RemoveEmptyEntries);
                var idWeigth = pieces[0].Split(new[] { '(', ')', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var id = idWeigth[0];

                ids.Add(id);

                if (pieces.Length > 1)
                {
                    var dependencies = pieces[1].Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var depId in dependencies)
                    {
                        children.Add(depId);
                    }
                }
            }

            ids.ExceptWith(children);
            return ids.First();
        }
    }
}
