using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace List3Exercise5b
{
    internal class Node
    {
        public Node(int integralValue)
        {
            this.BinaryCode = "";
            this.IntegralValue = integralValue;
            this.ByteValue = (byte)integralValue;
        }

        public Node(int integralValue, int quantity)
        {
            this.BinaryCode = "";
            this.ByteValue = (byte)integralValue;
            this.IntegralValue = integralValue;
            this.Quantity = quantity;
        }

        public Node(double probability)
        {
            this.BinaryCode = "";
            this.Probability = probability;
        }

        public int IntegralValue { get; set; }
        public byte ByteValue { get; set; }
        public double Probability { get; set; }
        public int Quantity { get; set; }
        public Node LeftChild { get; set; }
        public Node Parent { get; set; }
        public Node RightChild { get; set; }
        public string BinaryCode { get; set; }
    }

    internal static class HuffmanCoderMethods
    {
        private static void CalculateProbability(IReadOnlyCollection<Node> bytes)
        {
            long numOfBytes = bytes.Aggregate<Node, long>(0, (current, item) => current + item.Quantity);

            foreach (Node item in bytes)
            {
                item.Probability = (double)item.Quantity / numOfBytes;
            }
        }

        private static void PreOrder_Code(Node root, char codePiece, string previousRootCode)
        {
            if (root == null) return;
            root.BinaryCode += previousRootCode + codePiece;
            PreOrder_Code(root.LeftChild, '0', root.BinaryCode);
            PreOrder_Code(root.RightChild, '1', root.BinaryCode);
        }

        private static void PreOrder_Leaves(Node root, ICollection<Node> nodes)
        {
            if (root == null) return;
            if (root.LeftChild == null && root.RightChild == null) nodes.Add(root);
            PreOrder_Leaves(root.LeftChild, nodes);
            PreOrder_Leaves(root.RightChild, nodes);
        }

        public static string[] CreateHuffmanCodes(string path)
        {
            List<Node> nodes = new List<Node>();
            for (int i = 0; i < 256; i++)
            {
                nodes.Add(new Node(i, 0));
            }

            if (File.Exists(path))
            {
                byte[] bytearray = File.ReadAllBytes(path);

                foreach (byte t in bytearray)
                {
                    int indexInBytes = nodes.FindIndex(x => x.ByteValue == t);
                    if (indexInBytes != -1) nodes[indexInBytes].Quantity++;
                }
            }
            else
            {
                throw new Exception("This file don't exist");
            }

            CalculateProbability(nodes);

            while (nodes.Count > 1)
            {
                nodes = nodes.OrderBy(x => x.Probability).ToList();
                Node left = nodes.ElementAt(0);
                nodes.RemoveAt(0);
                Node right = nodes.ElementAt(0);
                nodes.RemoveAt(0);
                Node parent = new Node(left.Probability + right.Probability)
                {
                    RightChild = right,
                    LeftChild = left
                };
                parent.RightChild.Parent = parent;
                parent.LeftChild.Parent = parent;

                nodes.Add(parent);
            }

            Node root = nodes[0];
            nodes.RemoveAt(0);
            PreOrder_Code(root, '0', "");
            PreOrder_Leaves(root, nodes);
            nodes.ForEach(x => x.BinaryCode = x.BinaryCode.Substring(1));

            string[] codes = new string[256];
            foreach (Node node in nodes)
            {
                codes[node.IntegralValue] = node.BinaryCode;
            }
            return codes;
        }

        public static byte[] CompressFile(HuffmanCode code, string path)
        {
            byte[] fileBytes = File.ReadAllBytes(path);
            StringBuilder compressedCode = new StringBuilder();
            for (int i = 0; i < fileBytes.Length; i++)
            {
                compressedCode.Append(code.BinaryCodes[(int)fileBytes[i]]);
            }
            Debug.WriteLine($"Compressed Code: {compressedCode}");
            int numOfBytes = compressedCode.Length / 8;
            byte[] bytes = new byte[numOfBytes];
            for (int i = 0; i < numOfBytes; i++)
            {
                bytes[i] = Convert.ToByte(compressedCode.ToString(0, 8), 2);
                compressedCode.Remove(0, 8);
            }

            return bytes;
        }

        public static byte[] DecompressFile(HuffmanCode code, string path)
        {
            byte[] compressedFileBytes = File.ReadAllBytes(path);
            List<byte> originalFileBytes = new List<byte>();
            StringBuilder binaryCode = new StringBuilder();
            int length = 0;

            for (int i = 0; i < compressedFileBytes.Length; i++)
            {
                binaryCode.Append(Convert.ToString(compressedFileBytes[i], 2));
            }

            while (binaryCode.Length > length)
            {
                int index = code.BinaryCodes.ToList().IndexOf(binaryCode.ToString(0, length));
                if (index == -1)
                {
                    length++;
                }
                else
                {
                    originalFileBytes.Add((byte)index);
                    binaryCode.Remove(0, length);
                    length = 0;
                }
            }
            Debug.WriteLine($"Decompressed File Bytes\n{binaryCode}");
            return originalFileBytes.ToArray();
        }
    }
}