using System;
using System.Collections.Generic;
using System.Linq;

namespace List4Exercise4bver2
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
        public int Distributor { get; set; }
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
            int previousDistributor = 0;
            int distributor;

            foreach (Myletter item in list)
            {
                distributor = previousDistributor + item.Quantity;
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
        internal static bool CheckFirstBit(char first, char second)
        {
            return first == second;
        }

        internal static bool CheckSecondBit(char first, char second)
        {
            return (first == 1 && second == 0);
        }

        internal static int BinaryToIntegral(string binaryCode)
        {
            int integral = 0;
            for (int i = 0; i < binaryCode.Length; i++)
            {
                int twoToPower = (int)Math.Pow(2, i);
                int addOrNot = (int)Char.GetNumericValue(binaryCode[binaryCode.Length - 1 - i]);
                integral += twoToPower * addOrNot;
            }
            return integral;
        }

        internal static string IntegralToBinary(int number, int numOfBits)
        {
            string binary = "";

            if (number >= Math.Pow(2, numOfBits)) throw new Exception("number to big for this number of bits");

            for (int i = numOfBits - 1; i >= 0; i--)
            {
                if (number >= Math.Pow(2, i))
                {
                    binary += '1';
                    number -= (int)Math.Pow(2, i);
                }
                else binary += '0';
            }

            return binary;
        }

        internal static int SmallestPosibleRange(List<Myletter> letters)
        {
            int smallestRange = letters.Sum(x => x.Quantity);
            for (int i = 0; i < letters.Count; i++)
            {
                if (i == 0)
                {
                    if (letters[i].Distributor <= smallestRange) Math.Abs(smallestRange = letters[i].Distributor);
                }
                else if (Math.Abs(letters[i].Distributor - letters[i - 1].Distributor) <= smallestRange) Math.Abs(smallestRange = letters[i].Distributor - letters[i - 1].Distributor);
            }
            return smallestRange;
        }

        internal static int MinimumNumberOfBits(List<Myletter> letters, int smallestRange)
        {
            double k = letters.Sum(x => x.Quantity) / smallestRange * 4;

            int power = 0;
            while (k > Math.Pow(2, power)) power++;

            return power;
        }

        internal static string IntegralEncodingWithScaling(string codedWord, List<Myletter> letters)
        {
            string returnedCode = "";

            int numberOfBits = MinimumNumberOfBits(letters, SmallestPosibleRange(letters));
            int numOfLetters = letters.Sum(x => x.Quantity);
            List<string> allDString = new List<string>();
            List<string> allGString = new List<string>();
            List<int> allDDec = new List<int>();
            List<int> allGDec = new List<int>();

            int G = 0;

            allDString.Add(IntegralToBinary(0, numberOfBits));
            allGString.Add(IntegralToBinary((int)Math.Pow(2, numberOfBits) - 1, numberOfBits));
            allDDec.Add(0);
            allGDec.Add((int)Math.Pow(2, numberOfBits) - 1);

            //CONSOLE WRITE D AND G
            // Console.WriteLine($"D[0]={allDDec[0]}/{allDString[0]}");
            // Console.WriteLine($"G[0]={allGDec[0]}/{allGString[0]}");
            for (int n = 0; n < codedWord.Length; n++)
            {
                int newDDec, newGDec;

                int letterIndexInList = letters.FindIndex(x => x.Letter == codedWord[n]);

                if (letterIndexInList == 0)
                    newDDec = allDDec[^1] + (int)((allGDec[^1] - allDDec[^1] + 1) * 0 / numOfLetters);
                else
                    newDDec = allDDec[^1] + (int)((allGDec[^1] - allDDec[^1] + 1) * letters[letterIndexInList - 1].Distributor / numOfLetters);

                newGDec = allDDec[^1] + (int)((allGDec[^1] - allDDec[^1] + 1) * letters[letterIndexInList].Distributor / numOfLetters) - 1;

                string newDString = IntegralToBinary(newDDec, numberOfBits);
                string newGString = IntegralToBinary(newGDec, numberOfBits);

                //CONSOLE WRITE D AND G
                // Console.WriteLine($"D[{n + 1}]={newDDec}/{newDString}");
                //Console.WriteLine($"G[{n + 1}]={newGDec}/{newGString}");

                bool secondStageContinue = true;
                while (secondStageContinue)
                {
                    switch (CheckFirstBit(newDString[0], newGString[0]))
                    {
                        case true:
                            {
                                returnedCode += newDString[0];
                                // Console.WriteLine($"Code added {newDString[0]} Current Code {returnedCode}");
                                newDString = newDString.Substring(1) + '0';
                                newGString = newGString.Substring(1) + '1';
                                // Console.WriteLine($"Moved D[{n + 1}] {newDString}");
                                // Console.WriteLine($"Moved G[{n + 1}] {newGString}");
                                if (returnedCode[^1] == '0')
                                {
                                    returnedCode += string.Concat(Enumerable.Repeat("1", G));
                                    //      Console.WriteLine($"Code added '1' {G} times");
                                }
                                if (returnedCode[^1] == '1')
                                {
                                    returnedCode += string.Concat(Enumerable.Repeat("0", G));
                                    //     Console.WriteLine($"Code added '0' {G} times");
                                }

                                break;
                            }

                        case false:
                            {
                                secondStageContinue = false;
                                break;
                            }
                    }
                    if (!secondStageContinue)
                    {
                        switch (CheckSecondBit(newDString[1], newGString[1]))
                        {
                            case true:
                                {
                                    newDString = '0' + newDString.Substring(2) + '0';
                                    newGString = '1' + newGString.Substring(2) + '1';
                                    //  Console.WriteLine($"Moved D[{n + 1}] {newDString}");
                                    // Console.WriteLine($"Moved G[{n + 1}] {newGString}");
                                    G++;
                                    secondStageContinue = true;
                                    break;
                                }
                            case false:
                                {
                                    break;
                                }
                        }
                    }
                }

                allDString.Add(newDString);
                allGString.Add(newGString);
                newDDec = BinaryToIntegral(newDString);
                newGDec = BinaryToIntegral(newGString);
                allDDec.Add(newDDec);
                allGDec.Add(newGDec);
                //Console.WriteLine();
            }

            returnedCode += allDString[^1];
            return returnedCode;
        }
    }

    internal static class Decoder
    {
        internal static int CheckWhichDistributor(double sign, List<Myletter> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (sign < list[i].Distributor) return i;
            }
            return -1;
        }

        internal static string IntegralDecodingWithScaling(string code, List<Myletter> letters)
        {
            string returnedString = "";

            int numberOfBits = Encoder.MinimumNumberOfBits(letters, Encoder.SmallestPosibleRange(letters));
            // Console.WriteLine($"Number of bits={numberOfBits}");
            int numOfLetters = letters.Sum(x => x.Quantity);

            List<string> allDString = new List<string>();
            List<string> allGString = new List<string>();
            List<int> allDDec = new List<int>();
            List<int> allGDec = new List<int>();

            allDString.Add(Encoder.IntegralToBinary(0, numberOfBits));
            allGString.Add(Encoder.IntegralToBinary((int)Math.Pow(2, numberOfBits) - 1, numberOfBits));
            allDDec.Add(0);
            allGDec.Add((int)Math.Pow(2, numberOfBits) - 1);
            // Console.WriteLine($"D[0]= {allDDec[0]}    {allDString[0]}");
            //Console.WriteLine($"G[0]= {allGDec[0]}    {allGString[0]}");

            string windowedCode = code.Substring(0, numberOfBits);
            double sign = Encoder.BinaryToIntegral(windowedCode);
            //  Console.WriteLine($"Windowed code={windowedCode}    Sign={sign}");

            int nextReadedBit = numberOfBits;

            for (int n = 1; n < numOfLetters + 1; n++)
            {
                double tStar = (numOfLetters * (sign - allDDec[^1] + 1) - 1) / (allGDec[^1] - allDDec[^1] + 1);
                //  Console.WriteLine($"tStar={tStar}");
                int letterIndexInList = CheckWhichDistributor(tStar, letters);
                returnedString += letters[letterIndexInList].Letter;
                //  Console.WriteLine($"Actual code={returnedString}");

                int newDDec, newGDec;

                if (letterIndexInList == 0)
                    newDDec = allDDec[^1] + (int)((allGDec[^1] - allDDec[^1] + 1) * 0 / numOfLetters);
                else
                    newDDec = allDDec[^1] + (int)((allGDec[^1] - allDDec[^1] + 1) * letters[letterIndexInList - 1].Distributor / numOfLetters);

                newGDec = allDDec[^1] + (int)((allGDec[^1] - allDDec[^1] + 1) * letters[letterIndexInList].Distributor / numOfLetters) - 1;

                string newDString = Encoder.IntegralToBinary(newDDec, numberOfBits);
                string newGString = Encoder.IntegralToBinary(newGDec, numberOfBits);

                //CONSOLE WRITE D AND G
                //  Console.WriteLine($"D[{n}]={newDDec}/{newDString}");
                // Console.WriteLine($"G[{n}]={newGDec}/{newGString}");

                bool secondStageContinue = true;
                while (secondStageContinue)
                {
                    switch (Encoder.CheckFirstBit(newDString[0], newGString[0]))
                    {
                        case true:
                            {
                                newDString = newDString.Substring(1) + '0';
                                newGString = newGString.Substring(1) + '1';
                                // Console.WriteLine($"Moved D[{n}] {newDString}");
                                // Console.WriteLine($"Moved G[{n}] {newGString}");
                                windowedCode = windowedCode.Substring(1) + code[nextReadedBit];
                                sign = Encoder.BinaryToIntegral(windowedCode);
                                // Console.WriteLine($"D[{n}]={newDDec}/{newDString}");
                                //  Console.WriteLine($"G[{n}]={newGDec}/{newGString}");
                                // Console.WriteLine($"Windowed code={windowedCode}    Sign={sign}");
                                nextReadedBit++;
                                break;
                            }

                        case false:
                            {
                                secondStageContinue = false;
                                break;
                            }
                    }
                    if (!secondStageContinue)
                    {
                        switch (Encoder.CheckSecondBit(newDString[1], newGString[1]))
                        {
                            case true:
                                {
                                    newDString = newDString.Substring(1) + '0';
                                    newGString = newGString.Substring(1) + '1';
                                    //   Console.WriteLine($"Moved D[{n}] {newDString}");
                                    //  Console.WriteLine($"Moved G[{n}] {newGString}");
                                    windowedCode = windowedCode.Substring(1) + code[nextReadedBit];
                                    sign = Encoder.BinaryToIntegral(windowedCode);
                                    nextReadedBit++;
                                    if (newDString[0] == 1) newDString = '0' + newDString.Substring(1); else newDString = '1' + newDString.Substring(1);
                                    if (newGString[0] == 1) newGString = '0' + newGString.Substring(1); else newGString = '1' + newGString.Substring(1);
                                    if (windowedCode[0] == 1) windowedCode = '0' + windowedCode.Substring(1); else windowedCode = '1' + windowedCode.Substring(1);
                                    sign = Encoder.BinaryToIntegral(windowedCode);
                                    //  Console.WriteLine($"Negated D[{n}]={newDDec}/{newDString}");
                                    //  Console.WriteLine($"Negated G[{n}]={newGDec}/{newGString}");
                                    // Console.WriteLine($"Negated Windowed code={windowedCode}    Sign={sign}");

                                    secondStageContinue = true;
                                    break;
                                }
                            case false:
                                {
                                    break;
                                }
                        }
                    }
                }
                allDString.Add(newDString);
                allGString.Add(newGString);
                newDDec = Encoder.BinaryToIntegral(newDString);
                newGDec = Encoder.BinaryToIntegral(newGString);
                allDDec.Add(newDDec);
                allGDec.Add(newGDec);
            }

            return returnedString;
        }
    }

    internal class Program
    {
        private static void PrepareListToCode(List<Myletter> list)
        {
            Utilities.CalculateProbability(list);
            //Console.WriteLine($"Enthropy={Utilities.CalculateEnthropy(list)}");
            Utilities.CalculateDistributor(list);
            Utilities.WriteTable(list);
            Console.WriteLine();
        }

        private static void Main(string[] args)
        {
            List<Myletter> letters = new List<Myletter>();
            string enteredText = "";
            string sign;

            Console.Write("Please type in your text:");
            enteredText = Console.ReadLine();
            Console.WriteLine($"Your text is: {enteredText}");
            ////Labki przykład
            //enteredText = "abac";
            //letters.Add(new Myletter('a', 37));
            //letters.Add(new Myletter('b', 38));
            //letters.Add(new Myletter('c', 25));

            ////Wykład przykład
            //enteredText = "abac";
            //letters.Add(new Myletter('a', 37));
            //letters.Add(new Myletter('b', 38));
            //letters.Add(new Myletter('c', 25));

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

            sign = Encoder.IntegralEncodingWithScaling(enteredText, letters);

            Console.WriteLine($"Sign for entered text = {sign}\n");

            Console.Write("Would you like to decode this sign? y/n");
            Console.WriteLine();
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.Y:
                    {
                        string text = Decoder.IntegralDecodingWithScaling(sign, letters);
                        Console.WriteLine($"\nDecoded word for sign {sign} = {text}");
                    }
                    break;

                case ConsoleKey.N:
                    { Console.WriteLine("\nThank You for using this program."); }
                    break;

                default:
                    Console.WriteLine();
                    break;
            }

            Console.Write("Press any key to close program");
            Console.ReadKey();
        }
    }
}