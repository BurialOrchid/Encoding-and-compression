using System;
using System.IO;

namespace List6Exercise2
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            StreamReader reader = new StreamReader("../../../moje.txt");

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
        }
    }
}