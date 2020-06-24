using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace List2Exercise4b
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
                item.Probability = Math.Round((double)item.Quantity / numOfChars, 4);
            }

            return letters;
        }

        private static void WriteTable(List<Myletter> list)
        {
            foreach (Myletter item in list)
            {
                Console.WriteLine($"{item.Letter} - {item.Quantity} - {item.Probability}%");
            }
        }

        private static void Main(string[] args)
        {
            Random rand = new Random();

            StringBuilder tekst = new StringBuilder();
            Console.WriteLine("Calculating probability for each letter based on '4wyrazy.txt' file");
            List<Myletter> LettersFile = CalculateProbabilityModel("../../../4wyrazy.txt");
            WriteTable(LettersFile);

            double r = rand.NextDouble();

            for (int i = 0; i < 200; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    double sum = 0;
                    foreach (Myletter letter in LettersFile)
                    {
                        if (r < (sum + letter.Probability))
                        {
                            tekst.Append(letter.Letter);
                            r = rand.NextDouble();
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
            Console.WriteLine("Created file 'ProbabilityModelFrom4Words.txt'");
            Console.WriteLine("Click any key to go to project folder...");
            Console.ReadKey();

            Process.Start("explorer.exe", Directory.GetCurrentDirectory());

            Console.WriteLine("Click to close program");
            Console.ReadKey();
        }
    }
}