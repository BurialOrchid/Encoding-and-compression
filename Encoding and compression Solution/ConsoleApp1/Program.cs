using System;
using System.IO;
using System.Text;

namespace ConsoleApp1
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args is null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            Console.WriteLine("Hello World!");
            double entropy;

            entropy = -0.95 * Math.Log2(0.95) - 0.05 * Math.Log2(0.05);
            Console.WriteLine(entropy);
            Console.ReadKey();

            StringBuilder tekst = new StringBuilder();
            Random rand = new Random();

            double prawd = 0.5;
            for (int i = 0; i < 500000; i++)
            {
                double los = rand.NextDouble();
                if (los < prawd)
                {
                    tekst.Append('a');
                }
                else
                {
                    tekst.Append('b');
                }
            }

            File.WriteAllText(@"plik1.txt", tekst.ToString());

            tekst = new StringBuilder();
            rand = new Random();

            prawd = 0.75;
            for (int i = 0; i < 500000; i++)
            {
                double los = rand.NextDouble();
                if (los < prawd)
                {
                    tekst.Append('a');
                }
                else
                {
                    tekst.Append('b');
                }
            }

            File.WriteAllText(@"plik2.txt", tekst.ToString());

            tekst = new StringBuilder();
            rand = new Random();

            prawd = 0.9;
            for (int i = 0; i < 500000; i++)
            {
                double los = rand.NextDouble();
                if (los < prawd)
                {
                    tekst.Append('a');
                }
                else
                {
                    tekst.Append('b');
                }
            }

            File.WriteAllText(@"plik3.txt", tekst.ToString());

            tekst = new StringBuilder();
            rand = new Random();

            prawd = 0.95;
            for (int i = 0; i < 500000; i++)
            {
                double los = rand.NextDouble();
                if (los < prawd)
                {
                    tekst.Append('a');
                }
                else
                {
                    tekst.Append('b');
                }
            }

            File.WriteAllText(@"plik4.txt", tekst.ToString());

            tekst = new StringBuilder();
            rand = new Random();

            prawd = 1;
            for (int i = 0; i < 500000; i++)
            {
                double los = rand.NextDouble();
                if (los < prawd)
                {
                    tekst.Append('a');
                }
                else
                {
                    tekst.Append('b');
                }
            }

            File.WriteAllText(@"plik5.txt", tekst.ToString());
        }
    }
}