using System;

namespace EnthropyCalculator
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
            double first = 8 / 15;
            double second = 7 / 15;
            entropy = first * Math.Log2(1 / first) + second * Math.Log2(1 / second);
            Console.WriteLine(entropy);
            Console.ReadKey();

            double d = 0.312;
            double g = 0.6;
            double f = 0.82;
            Console.WriteLine(d + (g - d) * f);
            Console.ReadKey();
        }
    }
}