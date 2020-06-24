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
                            Console.WriteLine($"Letter-" + @"\n" + $"   Quantity-{item.Quantity}    Probability-{item.Probability}%");
                            break;
                        }
                    case ' ':
                        {
                            Console.WriteLine($"Letter-SPACE    Quantity-{item.Quantity}    Probability-{item.Probability}%");
                            break;
                        }
                    case '\r':
                        {
                            Console.WriteLine($"Letter-" + @"\r" + $"    Quantity -{item.Quantity}    Probability-{item.Probability}%");
                            break;
                        }
                    case '\t':
                        {
                            Console.WriteLine($"Letter-" + @"\t" + $"    Quantity-{item.Quantity}    Probability-{item.Probability}%");
                            break;
                        }
                    default:
                        {
                            Console.WriteLine($"Letter-{item.Letter}    Quantity-{item.Quantity}    Probability-{item.Probability}%");
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

        private static void Main(string[] args)
        {
            List<Myletter> letters = new List<Myletter>();
            StreamReader reader;
            string filename = "";

            do
            {
                Console.Clear();
                Console.WriteLine("Insert file name with extension");
                filename = Console.ReadLine();
                Console.WriteLine();
            }
            while (!File.Exists(filename));

            reader = new StreamReader($"{filename}");

            Console.WriteLine(reader.ReadToEnd() + '\n');
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

            Console.ReadKey();
        }
    }
}