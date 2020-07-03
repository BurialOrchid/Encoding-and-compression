using System;
using System.Collections.Generic;
using System.Linq;

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
                item.Distributor = distributor;
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
                Console.WriteLine($"Letter={item.Letter}    Quantity={item.Quantity}    Probability={item.Probability}%  Distirbutor={item.Distributor}");
            }
        }
    }

    internal static class Encoder
    {
        private static int CheckTheRange(double d, double g)
        {
            if (d >= 0 && g < 0.5) return 1;
            else if (d >= 0.5 && g <= 1) return 2;
            else return 3;
        }

        private static string CodeRandomInLastRange(double lowerend, double upperend, int precisionlength)
        {
            Random rnd = new Random();
            double toCode = lowerend + (upperend - lowerend);

            String binary = "";
            for (int i = 0; i < precisionlength; i++)
            {
                //Find next bit in fraction
                toCode *= 2;
                int fract_bit = (int)toCode;

                if (fract_bit == 1)
                {
                    toCode -= fract_bit;
                    binary += '1';
                }
                else
                {
                    binary += '0';
                }
            }
            return binary;
        }

        internal static string ArithmeticEncodingWithScaling(string codedWord, List<Myletter> list)
        {
            string returnedCode = "";
            List<double> alld = new List<double>();
            List<double> allg = new List<double>();

            alld.Add(0);
            allg.Add(1);

            string last;

            for (int i = 0; i < codedWord.Length; i++)
            {
                int letterIndexInList = list.FindIndex(x => x.Letter == codedWord[i]);
                double newd = 0;
                double newg = 0;
                // Calculate letters "d"
                if (letterIndexInList == 0) newd = (alld[^1] + (allg[^1] - alld[^1]) * 0);
                else
                    newd = (alld[^1] + (allg[^1] - alld[^1]) * list[letterIndexInList - 1].Distributor);

                //  Calculate letters "g"
                newg = (alld[^1] + (allg[^1] - alld[^1]) * list.Find(x => x.Letter == codedWord[i]).Distributor);

                bool endCheck = false;
                while (!endCheck)
                {
                    switch (CheckTheRange(newd, newg))
                    {
                        case 1:
                            {
                                returnedCode += '0';
                                newd *= 2;
                                newg *= 2;
                                break;
                            }
                        case 2:
                            {
                                returnedCode += '1';
                                newd = (newd - 0.5) * 2;
                                newg = (newg - 0.5) * 2;
                                break;
                            }
                        case 3:
                            {
                                if (i == codedWord.Length - 1)
                                {
                                    last = CodeRandomInLastRange(alld.Last(), allg.Last(), returnedCode.Length);
                                    returnedCode += last;
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
                alld.Add(newd);
                allg.Add(newg);
            }

            return returnedCode;
        }
    }

    internal static class Decoder
    {
        internal static double SmallestPosibleRange(List<Myletter> list)
        {
            double smallestRange = 1;
            for (int i = 0; i < list.Count; i++)
            {
                if (i == 0)
                {
                    if (list[i].Distributor <= smallestRange) smallestRange = list[i].Distributor;
                }
                else if (list[i].Distributor - list[i - 1].Distributor <= smallestRange) smallestRange = list[i].Distributor - list[i - 1].Distributor;
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
            return sign;
        }

        internal static int CheckWhichDistributor(double sign, List<Myletter> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (sign < list[i].Distributor) return i;
            }
            return -1;
        }

        private static int CheckTheRange(double d, double g)
        {
            if (d >= 0 && g < 0.5) return 1;
            else if (d >= 0.5 && g < 1) return 2;
            else return 3;
        }

        internal static string ArithmeticDecodingWithScaling(string code, List<Myletter> letters)
        {
            List<double> alld = new List<double>();
            List<double> allg = new List<double>();
            string returnedDecodedWord = "";

            int numOfBits = MinimumNumberOfBits(SmallestPosibleRange(letters));
            int numOfLetters = letters.Sum(x => x.Quantity);

            while (code.Length < numOfBits * 2) code += '0';
            Console.WriteLine($"Code {code} Length {code.Length}");

            string windowedcode;
            double sign;
            int whichDistributor;
            double tstar;

            alld.Add(0);
            allg.Add(1);

            int movecounter = 0;
            windowedcode = code.Substring(movecounter, numOfBits);
            movecounter++;
            sign = CodeToSignConversion(windowedcode, numOfBits);

            Console.WriteLine($"Window={windowedcode} Sign={sign}");

            while (returnedDecodedWord.Length <= numOfLetters)
            {
                Console.WriteLine();
                tstar = (sign - alld.Last()) / (allg.Last() - alld.Last());
                Console.WriteLine($"Tstar is {tstar}");
                whichDistributor = CheckWhichDistributor(tstar, letters);
                Console.WriteLine($"Distributor={whichDistributor}    Letter={letters[whichDistributor].Letter}");
                returnedDecodedWord += letters[whichDistributor].Letter;

                if (movecounter == numOfBits) break;

                double newd;
                double newg;
                if (letters.IndexOf(letters[whichDistributor]) - 1 < 0) newd = (alld.Last() + ((allg.Last() - alld.Last()) * 0));
                else
                    newd = alld.Last() + (allg.Last() - alld.Last()) * letters[whichDistributor - 1].Distributor;
                newg = alld.Last() + (allg.Last() - alld.Last()) * letters[whichDistributor].Distributor;

                Console.WriteLine($"D is {newd} G is {newg}");

                bool endCheck = false;
                while (!endCheck)
                {
                    switch (CheckTheRange(newd, newg))
                    {
                        case 1:
                            {
                                windowedcode = code.Substring(movecounter, numOfBits);
                                sign = CodeToSignConversion(windowedcode, numOfBits);
                                Console.WriteLine($"New window={windowedcode} New Sign={sign}");

                                newd *= 2;
                                newg *= 2;
                                Console.WriteLine($"Scaled D is {newd} G is {newg}");

                                movecounter++;
                                break;
                            }

                        case 2:
                            {
                                windowedcode = code.Substring(movecounter, numOfBits);
                                sign = CodeToSignConversion(windowedcode, numOfBits);
                                Console.WriteLine($"New window={windowedcode} New Sign={sign}");

                                newd = (newd - 0.5) * 2;
                                newg = (newg - 0.5) * 2;
                                Console.WriteLine($"Scaled D is {newd} G is {newg}");

                                movecounter++;
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
                alld.Add(newd);
                allg.Add(newg);
            }

            return returnedDecodedWord;
        }
    }

    internal class Program
    {
        private static void PrepareListToCode(List<Myletter> list)
        {
            Utilities.CalculateProbability(list);
            Console.WriteLine($"Enthropy={Utilities.CalculateEnthropy(list)}");
            Utilities.CalculateDistributor(list);
            Utilities.WriteTable(list);
        }

        private static void Main(string[] args)
        {
            List<Myletter> letters = new List<Myletter>();
            string enteredText = "";
            string sign;

            Console.Write("Please type in your text:");
            enteredText = Console.ReadLine();
            Console.WriteLine($"Your text is: {enteredText}");

            foreach (char character in enteredText)
            {
                int index = letters.FindIndex(x => x.Letter == character);
                if (index != -1)
                {
                    letters[index].Quantity++;
                }
                else
                {
                    letters.Add(new Myletter(character, 1));
                }
            }

            letters = letters.OrderBy(x => x.Letter).ToList();

            PrepareListToCode(letters);

            sign = Encoder.ArithmeticEncodingWithScaling(enteredText, letters);
            Console.WriteLine($"Sign for entered text = {sign}\n");

            Console.Write("Would you like to decode this sign? y/n");
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.Y:
                    {
                        Console.WriteLine();
                        string word = Decoder.ArithmeticDecodingWithScaling(sign, letters);
                        Console.WriteLine($"Decoded word for sign {sign} = {word}");
                    }
                    break;

                case ConsoleKey.N:

                    {
                        Console.WriteLine();
                        Console.WriteLine("Thank You for using this program.");
                    }
                    break;

                default:
                    break;
            }

            Console.Write("Press any key to close program");
            Console.ReadKey();
        }
    }
}