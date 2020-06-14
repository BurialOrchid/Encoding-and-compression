using System;
using System.Collections.Generic;
using System.Text;

namespace List3Exercise5
{
    internal partial class Program
    {
        private static void WriteTable(List<Myletter> list)
        {
            foreach (Myletter item in list)
            {
                switch (item.Character)
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
                            Console.WriteLine($"{item.Character} - {item.Quantity} - {item.Probability}");
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
                item.Probability = Math.Round((double)item.Quantity / numOfChars, 4);
            }
        }

        private static double CalculateEnthropy(List<Myletter> list)
        {
            double entropy = 0;
            foreach (Myletter item in list)
            {
                entropy += (item.Probability) * Math.Log2(1 / (item.Probability));
            }

            return entropy;
        }
    }
}