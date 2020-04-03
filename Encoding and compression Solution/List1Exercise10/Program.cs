using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace List1Exercise9
{
    internal class Program
    {
        private class Myletter
        {
            public Myletter(char letter, int num)
            {
                this.Letter = letter;
                this.Quantity = num;
            }

            public char Letter { get; set; }
            public int Quantity { get; set; }
            public double Probability { get; set; }
        }

        private static void WriteTable(List<Myletter> list)
        {
            foreach (Myletter item in list)
            {
                switch (item.Letter)
                {
                    case '\n':
                        {
                            Console.WriteLine($"NEWLINE - {item.Quantity} - {item.Probability}");
                            break;
                        }
                    case ' ':
                        {
                            Console.WriteLine($"SPACE - {item.Quantity} - {item.Probability}");
                            break;
                        }
                    default:
                        {
                            Console.WriteLine($"{item.Letter} - {item.Quantity} - {item.Probability}");
                            break;
                        }
                }
                Console.WriteLine("-----");
            }
        }

        private static void CalculateProbability(List<Myletter> list)
        {
            long numOfChars = 0;
            foreach (Myletter item in list)
            {
                numOfChars += item.Quantity;
            }
            foreach (Myletter item in list)
            {
                item.Probability = Math.Round((double)item.Quantity / numOfChars * 100, 2);
            }
        }

        private static double CalculateEnthropy(List<Myletter> list)
        {
            double entropy = 0;
            foreach (Myletter item in list)
            {
                entropy += (item.Probability / 100) * Math.Log2(1 / (item.Probability / 100));
            }

            return entropy;
        }

        private static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            List<Myletter> letters = new List<Myletter>();
            StreamReader reader = new StreamReader("../../../Green Eggs and Ham.txt");
            Console.WriteLine(reader.ReadToEnd());
            reader.DiscardBufferedData();
            reader.BaseStream.Seek(0, SeekOrigin.Begin);
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
            letters = letters.OrderBy(x => x.Quantity).ToList();
            letters.Reverse();
            CalculateProbability(letters);
            WriteTable(letters);
            Console.WriteLine(CalculateEnthropy(letters));

            Console.ReadKey();
        }
    }
}