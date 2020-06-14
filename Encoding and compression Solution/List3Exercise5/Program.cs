using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace List3Exercise5
{
    internal partial class Program
    {
        private static void Main(string[] args)
        {
            if (args is null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            List<Myletter> lettersList = new List<Myletter>();
            StreamReader reader = new StreamReader("../../../Green Eggs and Ham.txt");

            Console.WriteLine(reader.ReadToEnd());
            reader.DiscardBufferedData();
            reader.BaseStream.Seek(0, SeekOrigin.Begin);

            while (!reader.EndOfStream)
            {
                char nextchar = (char)reader.Read();
                int index = lettersList.FindIndex(x => x.Character == nextchar);
                if (index != -1)
                {
                    lettersList[index].Quantity++;
                }
                else
                {
                    lettersList.Add(new Myletter(nextchar, 1));
                }
            }

            lettersList = lettersList.OrderBy(x => x.Quantity).ToList();
            lettersList.Reverse();

            CalculateProbability(lettersList);

            WriteTable(lettersList);

            Console.WriteLine(CalculateEnthropy(lettersList));

            Console.ReadKey();
        }
    }
}