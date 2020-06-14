using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace List4Exercise1a
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
        public double Distributor { get; set; }
    }

    internal static class Coder
    {
        internal static double ArithmeticCoding(List<Myletter> list, string codedWord)
        {
            List<double> alld = new List<double>();
            List<double> allg = new List<double>();

            alld.Add(0);
            allg.Add(1);

            for (int i = 0; i < codedWord.Length; i++)
            {
                int prevIndex = list.FindIndex(x => x.Letter == codedWord[i]);
                if (prevIndex == 0) alld.Add(alld[i] + (allg[i] - alld[i]) * 0);
                else
                    alld.Add(alld[i] + (allg[i] - alld[i]) * list[prevIndex - 1].Distributor);
                // Console.WriteLine($"d{i}    {alld.Last()}");
                allg.Add(alld[i] + (allg[i] - alld[i]) * list.Find(x => x.Letter == codedWord[i]).Distributor);
                // Console.WriteLine($"g{i}    {allg.Last()}");
            }
            return (allg.Last() + alld.Last()) / 2;
        }
    }

    internal static class Decoder
    {
        internal static string ArithmeticDecoding(List<Myletter> list, double sign, int wordLength)
        {
            List<double> alld = new List<double>();
            List<double> allg = new List<double>();
            alld.Add(0);
            allg.Add(1);

            StringBuilder answer = new StringBuilder();

            double tstar;

            for (int k = 0; k < wordLength; k++)
            {
                tstar = (sign - alld[k]) / (allg[k] - alld[k]);
                for (int i = 0; i < list.Count; i++)
                {
                    double lowerLimit;
                    double upperLimit;
                    if (i - 1 < 0) lowerLimit = 0;
                    else lowerLimit = list[i - 1].Distributor;
                    upperLimit = list[i].Distributor;

                    if (lowerLimit <= tstar && tstar < upperLimit)
                    {
                        answer.Append(list[i].Letter);
                        if (list.IndexOf(list[i]) - 1 < 0) alld.Add(alld[k] + (allg[k] - alld[k]) * 0);
                        else
                            alld.Add(alld[k] + (allg[k] - alld[k]) * list[i - 1].Distributor);
                        allg.Add(alld[k] + (allg[k] - alld[k]) * list[i].Distributor);

                        break;
                    }
                }
            }
            return answer.ToString();
        }
    }

    internal class Program
    {
        private static double CalculateEnthropy(List<Myletter> list)
        {
            double entropy = 0;
            foreach (Myletter item in list)
            {
                entropy += (item.Probability / 100) * Math.Log2(1 / (item.Probability / 100));
            }

            return entropy;
        }

        private static void CalculateDistributor(List<Myletter> list)
        {
            double previousDistributor = 0;
            double distributor;

            foreach (Myletter item in list)
            {
                distributor = previousDistributor + item.Probability / 100;
                item.Distributor = Math.Round(distributor, 4);
                previousDistributor = distributor;
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
                item.Probability = (double)item.Quantity / numOfChars * 100;
            }
        }

        private static void WriteTable(List<Myletter> list)
        {
            foreach (Myletter item in list)
            {
                Console.WriteLine($"{item.Letter} - {item.Quantity} - {item.Probability} - {item.Distributor}");
            }
        }

        private static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            List<Myletter> letters = new List<Myletter>();
            StreamReader reader = new StreamReader("../../../Test.txt");
            Console.WriteLine(reader.ReadToEnd());
            reader.DiscardBufferedData();
            reader.BaseStream.Seek(0, SeekOrigin.Begin);
            while (!reader.EndOfStream)
            {
                char nextchar = (char)reader.Read();
                if (Char.IsLetterOrDigit(nextchar))
                {
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
            }
            // letters = letters.OrderBy(x => x.Quantity).ToList();
            // letters.Reverse();
            CalculateProbability(letters);
            CalculateDistributor(letters);
            WriteTable(letters);
            Console.WriteLine(CalculateEnthropy(letters));
            Console.WriteLine("-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-");

            string word = "acba";
            double sign = Coder.ArithmeticCoding(letters, word);
            Console.WriteLine($"Sign for word {word} = {sign}");
            Console.WriteLine($"Encoded word for sign {sign} is {Decoder.ArithmeticDecoding(letters, sign, word.Length)}");

            // Console.WriteLine($"10 letter code for sign 0.63215699");
            // Console.WriteLine(Decoder.ArithmeticDecoding(letters, 0.63215699, 10));

            Console.ReadKey();
        }
    }
}