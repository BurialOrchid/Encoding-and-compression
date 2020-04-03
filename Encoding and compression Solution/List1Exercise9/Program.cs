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
                this.Number = num;
            }

            public char Letter { get; set; }
            public int Number { get; set; }
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
                            Console.WriteLine($"NEWLINE - {item.Number} - {item.Probability}");
                            break;
                        }
                    case ' ':
                        {
                            Console.WriteLine($"SPACE - {item.Number} - {item.Probability}");
                            break;
                        }
                    default:
                        {
                            Console.WriteLine($"{item.Letter} - {item.Number} - {item.Probability}");
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
                numOfChars += item.Number;
            }
            foreach (Myletter item in list)
            {
                item.Probability = Math.Round((double)item.Number / numOfChars * 100, 2);
            }
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
                    letters[index].Number++;
                }
                else
                {
                    letters.Add(new Myletter(nextchar, 1));
                }
            }
            letters = letters.OrderBy(x => x.Number).ToList();
            letters.Reverse();
            CalculateProbability(letters);
            WriteTable(letters);

            Console.ReadKey();
        }
    }
}