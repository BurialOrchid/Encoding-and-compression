using System;

namespace QuickEnthopyCalk
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            double entropy = -((double)7 / (double)15 * Math.Log2((double)7 / (double)15)) - ((double)8 / (double)15 * Math.Log2((double)8 / (double)15));
            Console.WriteLine(entropy);
            Console.ReadKey();
        }
    }
}