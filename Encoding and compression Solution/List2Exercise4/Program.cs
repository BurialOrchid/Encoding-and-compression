using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace List2Exercise4
{
    internal class Program
    {
        private static readonly char[] PolishAlphabet = new char[] { 'a', 'ą', 'b', 'c', 'ć', 'd', 'e', 'ę', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'ł', 'm', 'n', 'ń', 'o', 'ó', 'p', 'r', 's', 'ś', 't', 'u', 'w', 'x', 'y', 'z', 'ź', 'ż' };

        private static void Main(string[] args)
        {
            if (args is null)
            {
                throw new ArgumentNullException(nameof(args));
            }

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

            Console.WriteLine("Created file 'sameprobability.txt'");
            Console.WriteLine("Click any key to go to project folder...");
            Console.ReadKey();

            Process.Start("explorer.exe", Directory.GetCurrentDirectory());

            Console.WriteLine("Click to close program");
            Console.ReadKey();

            //  WriteTable(CalculateProbabilityModel("sameprobability.txt"));
        }
    }
}