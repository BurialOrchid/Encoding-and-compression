using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace List2Exercise4
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

        private static List<Myletter> CalculateProbabilityModel(String Filepath)
        {
            List<Myletter> letters = new List<Myletter>();
            StreamReader reader = new StreamReader(Filepath);
            //Console.WriteLine(reader.ReadToEnd());
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
                    if (Char.IsLetter(nextchar))
                        letters.Add(new Myletter(nextchar, 1));
                }
            }
            letters = letters.OrderBy(x => x.Quantity).ToList();
            letters.Reverse();

            long numOfChars = 0;
            foreach (Myletter item in letters)
            {
                numOfChars += item.Quantity;
            }
            foreach (Myletter item in letters)
            {
                item.Probability = (double)item.Quantity / numOfChars;
            }

            return letters;
        }

        private static void WriteTable(List<Myletter> list)
        {
            foreach (Myletter item in list)
            {
                Console.WriteLine($"{item.Letter} - {item.Quantity} - {item.Probability}");
            }
        }

        private static char[] PolishAlphabet = new char[] { 'a', 'ą', 'b', 'c', 'ć', 'd', 'e', 'ę', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'ł', 'm', 'n', 'ń', 'o', 'ó', 'p', 'r', 's', 'ś', 't', 'u', 'w', 'x', 'y', 'z', 'ź', 'ż' };

        private static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Random rand = new Random();
            StringBuilder tekst;
            tekst = new StringBuilder();
            for (int i = 0; i < 200; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    tekst.Append(PolishAlphabet[rand.Next(0, PolishAlphabet.Length)]);
                }
                tekst.Append("\n");
            }
            File.WriteAllText(@"sameprobability.txt", tekst.ToString());
            Console.WriteLine("done writing file with same probability for each letter...");
            Console.ReadKey();

            //  WriteTable(CalculateProbabilityModel("sameprobability.txt"));

            List<Myletter> LettersFile = CalculateProbabilityModel("../../../4wyrazy.txt");
            WriteTable(LettersFile);
            double sum = 0;
            double r = rand.NextDouble();
            tekst = new StringBuilder();
            for (int i = 0; i < 200; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    foreach (Myletter letter in LettersFile)
                    {
                        if (r < (sum + letter.Probability))
                        {
                            tekst.Append(PolishAlphabet[rand.Next(0, PolishAlphabet.Length)]);

                            break;
                        }
                        else
                        {
                            sum += letter.Probability;
                        }
                    }
                }
                tekst.Append("\n");
            }
            File.WriteAllText(@"ProbabilityModelFrom4Words.txt", tekst.ToString());

            Console.ReadKey();
        }
    }
}