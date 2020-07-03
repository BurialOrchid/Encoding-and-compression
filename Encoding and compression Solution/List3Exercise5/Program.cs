using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace List3Exercise5a
{
    internal class Myletter
    {
        public Myletter(char letter, int num)
        {
            this.Letter = letter;
            this.Quantity = num;
        }

        public char Letter { get; set; }
        public int Quantity { get; set; }
        public double Probability { get; set; }
        public string BinaryCode { get; set; }
    }

    internal class Node
    {
        public Node(Myletter letter, double probability)
        {
            this.Letter = letter;
            this.Probability = probability;
            this.BinaryCode = "";
        }

        public Node Parent { get; set; }
        public double Probability { get; set; }
        public Myletter Letter { get; set; }
        public Node LeftChild { get; set; }
        public Node RightChild { get; set; }
        public string BinaryCode { get; set; }
    }

    internal partial class Program
    {
        private static void CalculateProbability(List<Myletter> letters)
        {
            long numOfChars = 0;
            foreach (Myletter item in letters)
            {
                numOfChars += item.Quantity;
            }
            foreach (Myletter item in letters)
            {
                item.Probability = Math.Round((double)item.Quantity / numOfChars, 4);
            }
        }

        private static double CalculateEnthropy(List<Myletter> letters)
        {
            double entropy = 0;
            foreach (Myletter item in letters)
            {
                entropy += (item.Probability) * Math.Log2(1 / (item.Probability));
            }

            return entropy;
        }

        private static void PreOrder_Code(Node root, char codePiece, string prevrootcode)
        {
            if (root != null)
            {
                root.BinaryCode += prevrootcode + codePiece;
                PreOrder_Code(root.LeftChild, '0', root.BinaryCode);
                PreOrder_Code(root.RightChild, '1', root.BinaryCode);
            }
        }

        private static void PreOrder_Leaves(Node root, List<Node> nodes)
        {
            if (root != null)
            {
                if (root.Letter != null) nodes.Add(root);
                PreOrder_Leaves(root.LeftChild, nodes);
                PreOrder_Leaves(root.RightChild, nodes);
            }
        }

        private static double MeanCodeLength(List<Myletter> letters)
        {
            int sumOfLetters = letters.Sum(x => x.Quantity);
            int codesum = letters.Sum(x => x.Quantity * x.BinaryCode.Length);
            return (double)codesum / sumOfLetters;
        }

        private static void Main(string[] args)
        {
            List<Myletter> letters = new List<Myletter>();
            List<Node> nodes = new List<Node>();

            StreamReader reader = new StreamReader("../../../4wyrazy.txt");

            while (!reader.EndOfStream)
            {
                char nextchar = (char)reader.Read();
                int index = letters.FindIndex(x => x.Letter == nextchar);
                if (index != -1)
                {
                    letters[index].Quantity++;
                }
                else
                {
                    letters.Add(new Myletter(nextchar, 1));
                }
            }
            CalculateProbability(letters);

            double enthropy = CalculateEnthropy(letters);
            Console.WriteLine($"Enthropy {enthropy}\n");

            letters.ForEach(x => nodes.Add(new Node(x, x.Probability)));
            //Console.WriteLine("Nodes");
            //nodes.ForEach(x => Console.WriteLine(x.Probability));

            while (nodes.Count > 1)
            {
                nodes = nodes.OrderBy(x => x.Probability).ToList();
                Node left = nodes.ElementAt(0);
                nodes.RemoveAt(0);
                Node right = nodes.ElementAt(0);
                nodes.RemoveAt(0);
                Node parent = new Node(null, left.Probability + right.Probability);
                parent.RightChild = right;
                parent.RightChild.Parent = parent;
                parent.LeftChild = left;
                parent.LeftChild.Parent = parent;
                nodes.Add(parent);
            }

            Node root = nodes[0];
            nodes.RemoveAt(0);
            PreOrder_Code(root, '0', "");
            PreOrder_Leaves(root, nodes);
            nodes.ForEach(x => x.BinaryCode = x.BinaryCode.Substring(1));

            letters.ForEach(x => x.BinaryCode = nodes.Find(y => y.Letter.Letter == x.Letter).BinaryCode);

            letters.ForEach(x => Console.WriteLine($"Letter-{x.Letter}  Code-{x.BinaryCode}"));
            double meanCodeLength = MeanCodeLength(letters);
            Console.WriteLine($"Mean code length:{meanCodeLength}");
            Console.WriteLine($"Code Redundancy{meanCodeLength - enthropy}");
            Console.WriteLine($"\nPress any key to close program");
            Console.ReadKey();
        }
    }
}