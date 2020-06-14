using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace List4Exercise4b
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

    internal static class Utilities
    {
        internal static double CalculateEnthropy(List<Myletter> list)
        {
            double entropy = 0;
            foreach (Myletter item in list)
            {
                entropy += (item.Probability / 100) * Math.Log2(1 / (item.Probability / 100));
            }

            return entropy;
        }

        internal static void CalculateDistributor(List<Myletter> list)
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

        internal static void CalculateProbability(List<Myletter> list)
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

        internal static void WriteTable(List<Myletter> list)
        {
            foreach (Myletter item in list)
            {
                Console.WriteLine($"{item.Letter} - {item.Quantity} - {item.Probability} - {item.Distributor}");
            }
        }
    }

    internal static class Encoder
    {
        private static int CheckTheRange(double d, double g)
        {
            if (d >= 0 && g < 0.5) return 1;
            else if (d >= 0.5 && g < 1) return 2;
            else return 3;
        }

        private static string CodeRandomInLastRange(double lowerend, double upperend, int precisionlength)
        {
            Random rnd = new Random();
            double toCode = lowerend + (upperend - lowerend) * rnd.NextDouble();
            toCode = .5;
            String binary = "";

            int Integral = (int)toCode;
            if (Integral > 0) throw new Exception("Random number in last range is more than 1");
            double fractional = toCode - Integral;

            char[] arr = binary.ToCharArray();
            Array.Reverse(arr);
            binary = new string(arr);

            // Append point before conversion of
            // fractional part

            // Conversion of fractional part to
            // binary equivalent
            while (precisionlength-- > 0)
            {
                // Find next bit in fraction
                fractional *= 2;
                int fract_bit = (int)fractional;

                if (fract_bit == 1)
                {
                    fractional -= fract_bit;
                    binary += (char)(1 + '0');
                }
                else
                {
                    binary += (char)(0 + '0');
                }
            }
            //  Console.WriteLine($"Lower end {lowerend}, upper end {upperend}, random {toCode}, as binary {binary}");
            return binary;
        }

        internal static string ArithmeticEncodingWithScaling(string codedWord, List<Myletter> list)
        {
            List<int> returnedCode = new List<int>();
            List<double> alld = new List<double>();
            List<double> allg = new List<double>();

            alld.Add(0);
            allg.Add(1);

            string last;

            for (int i = 0; i < codedWord.Length; i++)
            {
                int letterIndexInList = list.FindIndex(x => x.Letter == codedWord[i]);

                //Calculate letters "d"
                if (letterIndexInList == 0) alld.Add(alld[i] + (allg[i] - alld[i]) * 0);
                else
                    alld.Add(alld[i] + (allg[i] - alld[i]) * list[letterIndexInList - 1].Distributor);

                //Calculate letters "g"
                allg.Add(alld[i] + (allg[i] - alld[i]) * list.Find(x => x.Letter == codedWord[i]).Distributor);

                bool endCheck = false;
                while (!endCheck)
                {
                    switch (CheckTheRange(alld.Last(), allg.Last()))
                    {
                        case 1:
                            {
                                returnedCode.Add(0);
                                alld[^1] = alld[^1] * 2;
                                allg[^1] = allg[^1] * 2;
                                break;
                            }
                        case 2:
                            {
                                returnedCode.Add(1);
                                alld[^1] = (alld[^1] - 0.5) * 2;
                                allg[^1] = (allg[^1] - 0.5) * 2;
                                break;
                            }
                        case 3:
                            {
                                if (i == codedWord.Length - 1)
                                {
                                    last = CodeRandomInLastRange(alld.Last(), allg.Last(), codedWord.Length);
                                    for (int j = 0; j < last.Length; j++)
                                    {
                                        returnedCode.Add((int)Char.GetNumericValue(last[j]));
                                    }
                                }
                                endCheck = true;
                                break;
                            }
                        default:
                            {
                                Console.WriteLine("Something isn't working in CheckTheRange function");
                                break;
                            }
                    }
                }
            }

            return string.Join("", returnedCode.Select(x => x.ToString()).ToArray());
        }
    }

    internal static class Decoder
    {
        internal static double SmallestPosibleRange(List<Myletter> list)
        {
            double smallestRange = 1;
            for (int i = 1; i < list.Count; i++)
            {
                if (list[i].Distributor - list[i - 1].Distributor <= smallestRange) smallestRange = list[i].Distributor - list[i - 1].Distributor;
            }
            return smallestRange;
        }

        internal static int MinimumNumberOfBits(double smallestRange)
        {
            double k = (-Math.Log2(smallestRange)) + 1;
            return (int)k;
        }

        internal static double CodeToSignConversion(string code, int numofbits)
        {
            double sign = 0;
            double powerOfTwo = 0;
            for (int i = numofbits - 1; i >= 0; i--)
            {
                double number = Char.GetNumericValue(code[i]);
                sign += number * Math.Pow(2, powerOfTwo);
                powerOfTwo++;
            }
            sign /= Math.Pow(2, numofbits);
            // Console.WriteLine($"Sign is {sign}");
            return sign;
        }

        internal static int CheckWhichDistributor(double sign, List<Myletter> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (sign <= list[i].Distributor) return i;
            }
            return -1;
        }

        private static int CheckTheRange(double d, double g)
        {
            if (d >= 0 && g < 0.5) return 1;
            else if (d >= 0.5 && g < 1) return 2;
            else return 3;
        }

        internal static string ArithmeticDecodingWithScaling(string code, List<Myletter> list)
        {
            List<double> alld = new List<double>();
            List<double> allg = new List<double>();

            StringBuilder returnedDecodedWord = new StringBuilder();

            int k = MinimumNumberOfBits(SmallestPosibleRange(list));

            while (code.Length < k * 2) code += '0';
            //  Console.WriteLine($"Appendned code  {code} Length {code.Length}");

            string windowedcode;
            double sign;
            int whichDistributor;
            double tstar;

            alld.Add(0);
            allg.Add(1);

            int movecounter = 0;
            windowedcode = code.Substring(movecounter, k);

            //Console.WriteLine($"Windowed code = {windowedcode}");

            sign = CodeToSignConversion(windowedcode, k);
            tstar = sign;
            // Console.WriteLine($"Sign equals = {sign}");

            while (true)
            {
                whichDistributor = CheckWhichDistributor(tstar, list);
                //  Console.WriteLine($"Calculated distributor is {whichDistributor}");
                //  Console.WriteLine($"Calculated letter is {list[whichDistributor].Letter}");

                returnedDecodedWord.Append(list[whichDistributor].Letter);
                if (whichDistributor == -1) throw new Exception("Something isn't working in checkWhichDistributor method");

                if (movecounter == k) break;

                double newd;
                double newg;
                if (list.IndexOf(list[whichDistributor]) - 1 < 0) newd = (alld.Last() + ((allg.Last() - alld.Last()) * 0));
                else
                    newd = (Math.Round(alld.Last() + (allg.Last() - alld.Last()) * list[whichDistributor - 1].Distributor, k));
                newg = (Math.Round(alld.Last() + (allg.Last() - alld.Last()) * list[whichDistributor].Distributor, k));
                alld.Add(newd);
                allg.Add(newg);
                // Console.WriteLine($"Calculated D is {alld.Last()}");
                //  Console.WriteLine($"Calculated G is {allg.Last()}");

                bool endCheck = false;
                while (!endCheck)
                {
                    switch (CheckTheRange(alld.Last(), allg.Last()))
                    {
                        case 1:
                            {
                                movecounter++;
                                windowedcode = code.Substring(movecounter, k);
                                //  Console.WriteLine($"New windowed code = {windowedcode}");
                                alld[^1] = Math.Round(alld[^1] * 2, k);
                                //   Console.WriteLine($"Calculated D is {alld.Last()}");
                                allg[^1] = Math.Round(allg[^1] * 2, k);
                                //  Console.WriteLine($"Calculated G is {allg.Last()}");
                                sign = CodeToSignConversion(windowedcode, k);
                                // Console.WriteLine($"Calculated new sign is {sign}");
                                break;
                            }

                        case 2:
                            {
                                movecounter++;
                                windowedcode = code.Substring(movecounter, k);
                                //  Console.WriteLine($"New windowed code = {windowedcode}");
                                alld[^1] = Math.Round((alld[^1] - 0.5) * 2, k);
                                //  Console.WriteLine($"Calculated D is {alld.Last()}");
                                allg[^1] = Math.Round((allg[^1] - 0.5) * 2, k);
                                // Console.WriteLine($"Calculated G is {allg.Last()}");
                                sign = CodeToSignConversion(windowedcode, k);
                                // Console.WriteLine($"Calculated new sign is {sign}");
                                break;
                            }
                        case 3:
                            {
                                endCheck = true;
                                break;
                            }
                        default:
                            {
                                Console.WriteLine("Something isn't working in CheckTheRange function");
                                break;
                            }
                    }
                }

                tstar = Math.Round((sign - alld.Last()) / (allg.Last() - alld.Last()), k);
                //  Console.WriteLine($"Tstar is {tstar}");
                // Console.WriteLine();
            }

            return returnedDecodedWord.ToString();
        }
    }

    internal class Program
    {
        private static void PrepareListToCode(List<Myletter> list)
        {
            Utilities.CalculateProbability(list);
            Utilities.CalculateEnthropy(list);
            Utilities.CalculateDistributor(list);
            // Utilities.WriteTable(list);
        }

        private static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            List<Myletter> letters = new List<Myletter>();
            StreamReader reader = new StreamReader("../../../Test.txt");

            //  Console.WriteLine(reader.ReadToEnd());

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

            PrepareListToCode(letters);

            string textToCode = "acba";
            string code = Encoder.ArithmeticEncodingWithScaling(textToCode, letters);
            Console.WriteLine($"Code for {textToCode} is {code}");
            string codeToText = Decoder.ArithmeticDecodingWithScaling(code, letters);
            Console.WriteLine($"Decoded message from code {code} is {codeToText}");
            Console.ReadKey();
        }
    }
}