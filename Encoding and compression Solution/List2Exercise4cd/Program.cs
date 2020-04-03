using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace List2Exercise4c
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

        private class NextLetter : Myletter
        {
            public NextLetter(char letter, int num, char previousChar) : base(letter, num)
            {
                PreviousLetters = new List<Myletter>();
                //ProbabilityBasedOnLetter = new List<double>();
                PreviousLetters.Add(new Myletter(previousChar, 1));
            }

            public List<Myletter> PreviousLetters { get; set; }
            //public List<double> ProbabilityBasedOnLetter { get; set; }

            public void AddPreviousLetter(char previous)
            {
                int index = PreviousLetters.FindIndex(x => x.Letter == previous);
                if (index != -1)
                {
                    PreviousLetters[index].Quantity++;
                }
                else
                {
                    PreviousLetters.Add(new Myletter(previous, 1));
                }
            }

            public void CalcuclateProbabilityBasedOnPrevious()
            {
                int sumOfPreviousLetters = PreviousLetters.Sum(x => x.Quantity);

                foreach (Myletter previousLetter in PreviousLetters)
                {
                    previousLetter.Probability = ((double)previousLetter.Quantity / sumOfPreviousLetters * Probability);
                }
            }

            public void WriteProbabilityTable()
            {
                Console.WriteLine($"FOR LETTER {this.Letter} with probability {this.Probability}");
                foreach (Myletter item in PreviousLetters)
                {
                    Console.WriteLine($"{item.Letter}-letter {item.Quantity}-quant. {item.Probability}-prob. ");
                }
                Console.WriteLine();
            }
        }

        private static List<Myletter> CalculatePropbabilityForFirstLetter(string Path)
        {
            List<Myletter> FirstLetters = new List<Myletter>();
            StreamReader reader = new StreamReader(Path);

            //Console.WriteLine(reader.ReadToEnd());

            while (!reader.EndOfStream)
            {
                char nextchar = (char)reader.Read();
                if (Char.IsLetter(nextchar))
                {
                    int index = FirstLetters.FindIndex(x => x.Letter == nextchar);
                    if (index != -1)
                    {
                        FirstLetters[index].Quantity++;
                    }
                    else
                    {
                        FirstLetters.Add(new Myletter(nextchar, 1));
                    }
                }
                for (int i = 0; i < 5; i++)
                {
                    reader.Read();
                }
            }
            FirstLetters = FirstLetters.OrderBy(x => x.Quantity).ToList();
            FirstLetters.Reverse();

            long numOfChars = FirstLetters.Sum(x => x.Quantity);
            foreach (Myletter item in FirstLetters)
            {
                item.Probability = (double)item.Quantity / numOfChars;
            }
            return FirstLetters;
        }

        private static List<NextLetter> CalculatePropbabilityForNextLetter(string Path, int numofletter)
        {
            List<NextLetter> SecondLetters = new List<NextLetter>();
            StreamReader reader = new StreamReader(Path);
            while (!reader.EndOfStream)
            {
                for (int i = 0; i < numofletter - 2; i++)
                {
                    reader.Read();
                }
                char previouschar = (char)reader.Read();
                char targetchar = (char)reader.Read();
                int index = SecondLetters.FindIndex(x => x.Letter == targetchar);
                if (index != -1)
                {
                    SecondLetters[index].Quantity++;
                    SecondLetters[index].AddPreviousLetter(previouschar);
                }
                else
                {
                    SecondLetters.Add(new NextLetter(targetchar, 1, previouschar));
                }
                for (int i = 0; i < 6 - numofletter; i++)
                {
                    reader.Read();
                }
            }

            long numOfChars = SecondLetters.Sum(x => x.Quantity);

            foreach (NextLetter item in SecondLetters)
            {
                item.Probability = (double)item.Quantity / numOfChars;
                item.CalcuclateProbabilityBasedOnPrevious();
            }
            SecondLetters = SecondLetters.OrderBy(x => x.Quantity).ToList();
            SecondLetters.Reverse();
            return SecondLetters;
        }

        private static void WriteTable(List<Myletter> list)
        {
            foreach (Myletter item in list)
            {
                Console.WriteLine($"{item.Letter} - {item.Quantity} - {item.Probability}");
            }
        }

        private static void WriteTable(List<NextLetter> list)
        {
            foreach (Myletter item in list)
            {
                Console.WriteLine($"{item.Letter} - {item.Quantity} - {item.Probability}");
            }
        }

        private static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            List<Myletter> FirstLetters = CalculatePropbabilityForFirstLetter("../../../4wyrazy.txt");
            WriteTable(FirstLetters);
            Console.WriteLine();
            List<NextLetter> SecondLetters = CalculatePropbabilityForNextLetter("../../../4wyrazy.txt", 2);
            WriteTable(SecondLetters);
            Console.WriteLine();
            List<NextLetter> ThirdLetters = CalculatePropbabilityForNextLetter("../../../4wyrazy.txt", 3);
            WriteTable(ThirdLetters);
            Console.WriteLine();
            List<NextLetter> FourthLetters = CalculatePropbabilityForNextLetter("../../../4wyrazy.txt", 4);
            WriteTable(FourthLetters);
            Console.WriteLine();
            Random rand = new Random();
            StringBuilder tekst;
            tekst = new StringBuilder();
            double sum;
            double r;
            char previouschar = ' ';

            for (int i = 0; i < 200; i++)
            {
                tekst = new StringBuilder();

                sum = 0;
                r = rand.NextDouble();
                foreach (Myletter myletter in FirstLetters)
                {
                    if (r < (sum + myletter.Probability))
                    {
                        tekst.Append(myletter.Letter);
                        previouschar = myletter.Letter;
                        break;
                    }
                    else
                    {
                        sum += myletter.Probability;
                    }
                }
                r = rand.NextDouble();
                sum = 0;

                foreach (NextLetter nextletter in SecondLetters)
                {
                    foreach (Myletter prev in nextletter.PreviousLetters)
                    {
                        if (r < (sum + prev.Probability))
                        {
                            tekst.Append(nextletter.Letter);
                            break;
                        }
                        else
                        {
                            sum += +prev.Probability;
                        }
                    }
                    if (tekst.Length >= 2) break;
                }
                r = rand.NextDouble();
                sum = 0;
                foreach (NextLetter nextletter in ThirdLetters)
                {
                    foreach (Myletter prev in nextletter.PreviousLetters)
                    {
                        if (r < (sum + prev.Probability))
                        {
                            tekst.Append(nextletter.Letter);
                            break;
                        }
                        else
                        {
                            sum += +prev.Probability;
                        }
                    }
                    if (tekst.Length >= 3) break;
                }
                r = rand.NextDouble();
                sum = 0;
                foreach (NextLetter nextletter in FourthLetters)
                {
                    foreach (Myletter prev in nextletter.PreviousLetters)
                    {
                        if (r < (sum + prev.Probability))
                        {
                            tekst.Append(nextletter.Letter);
                            break;
                        }
                        else
                        {
                            sum += +prev.Probability;
                        }
                    }
                    if (tekst.Length >= 4) break;
                }
                Console.WriteLine(tekst);
            }

            Console.ReadKey();
        }
    }
}