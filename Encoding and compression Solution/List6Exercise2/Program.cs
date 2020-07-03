using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace List6Exercise2
{
    public class Letter
    {
        public Letter(char character, int quantity)
        {
            this.Quantity = quantity;
            this.Character = character;
        }

        public char Character { get; set; }
        public int Quantity { get; set; }
        public string Code { get; set; }

        public override string ToString()
        {
            return $"Character:{Character}   Quantity:{Quantity}";
        }
    }

    public class Digram
    {
        public Digram(string digramValue, int quantity)
        {
            Quantity = quantity;
            DigramValue = digramValue;
        }

        public int Quantity { get; set; }
        public string Code { get; set; }
        public string DigramValue { get; set; }

        public override string ToString()
        {
            return $"Digram:{DigramValue}   Quantity:{Quantity}";
        }
    }

    public class Dictionary
    {
        public int BiteSize { get; set; }
        public List<Digram> Digrams { get; set; }
        public List<Letter> Characters { get; set; }
        public List<string> AllSigns { get; set; }
        public List<string> SignCodes { get; set; }
        public double Capacity { get; set; }

        public Dictionary()
        {
            Digrams = new List<Digram>();
            Characters = new List<Letter>();
            AllSigns = new List<string>();
            SignCodes = new List<string>();
        }

        public int MinBiteSize()
        {
            int power = 0;
            while (Characters.Count > Math.Pow(2, power)) power++;
            return power;
        }

        public int MaxBiteSize()
        {
            int power = 0;
            while (Characters.Count + Digrams.Count > Math.Pow(2, power)) power++;
            return power;
        }
    }

    internal class Program
    {
        private static void DigramCode(Dictionary dict, string inputPath, string outputPath)
        {
            StreamReader reader = new StreamReader(inputPath);
            StringBuilder builder = new StringBuilder();
            char previousLetter = (char)reader.Read();
            //read all characters, populate dictionary.characters and dictionary.digrams
            while (!reader.EndOfStream)
            {
                var nextLetter = (char)reader.Read();
                int index = dict.AllSigns.IndexOf($"{previousLetter}{nextLetter}");
                builder.Append(index != -1
                    ? dict.SignCodes[index]
                    : dict.SignCodes[dict.AllSigns.IndexOf($"{previousLetter}")]);

                previousLetter = nextLetter;
            }

            int numOfBytes = builder.Length / 8;
            byte[] bytes = new byte[numOfBytes];
            for (int i = 0; i < numOfBytes; i++)
            {
                bytes[i] = Convert.ToByte(builder.ToString(0, 8), 2);
                builder.Remove(0, 8);
            }
            File.WriteAllBytes(outputPath, bytes);
        }

        private static void Main(string[] args)
        {
            Dictionary dictionary = new Dictionary();
            string filename;
            string inputFilePath;
            string outputFilePath;
            int minDictionarySize, maxDictionarySize, selectedDictionarySize = 0;
            StreamReader reader;
            do
            {
                Console.Write($"Digram Coding\nPlease select file to code:");
                filename = Console.ReadLine();
                Console.Clear();
            } while (!File.Exists($"../../../{filename}.txt"));

            inputFilePath = $"../../../{filename}.txt";

            reader = new StreamReader(inputFilePath);

            char previousLetter = (char)reader.Read();
            dictionary.Characters.Add(new Letter(previousLetter, 1));

            //read all characters, populate dictionary.characters and dictionary.digrams
            while (!reader.EndOfStream)
            {
                char nextLetter = (char)reader.Read();
                int index = dictionary.Characters.FindIndex(x => x.Character == nextLetter);
                if (index != -1)
                {
                    dictionary.Characters[index].Quantity++;
                }
                else dictionary.Characters.Add(new Letter(nextLetter, 1));

                string digram = $"{previousLetter}{nextLetter}";

                index = dictionary.Digrams.FindIndex(x => x.DigramValue == digram);
                if (index != -1)
                {
                    dictionary.Digrams[index].Quantity++;
                }
                else dictionary.Digrams.Add(new Digram(digram, 1));

                previousLetter = nextLetter;
            }

            Console.WriteLine($"Characters number: {dictionary.Characters.Count}");
            Console.WriteLine($"Digrams number: {dictionary.Digrams.Count}");

            minDictionarySize = dictionary.MinBiteSize();
            maxDictionarySize = dictionary.MaxBiteSize();

            Console.WriteLine($"Minimum dictionary size: {minDictionarySize}");
            Console.WriteLine($"Maximum dictionary size: {maxDictionarySize}");

            //select dictionary capacity, biteSize
            while (selectedDictionarySize < minDictionarySize || selectedDictionarySize > maxDictionarySize)
            {
                Console.Write($"Select dictionary size:");
                int.TryParse(Console.ReadLine(), out selectedDictionarySize);
            }
            dictionary.BiteSize = selectedDictionarySize;
            dictionary.Capacity = Math.Pow(2, selectedDictionarySize);

            //add all characters to main dictionary
            dictionary.Characters = dictionary.Characters.OrderBy(x => x.Character).ToList();
            for (int i = 0; i < dictionary.Characters.Count; i++)
            {
                dictionary.AllSigns.Add($"{dictionary.Characters[i].Character}");
                dictionary.SignCodes.Add(Convert.ToString(i, 2).PadLeft(8, '0'));
                // Console.WriteLine($"Letter:{dictionary.AllSigns[i]}     Code:{dictionary.SignCodes[i]}");
            }

            //sort digrams based on quantity
            dictionary.Digrams = dictionary.Digrams.OrderBy(x => x.Quantity).Reverse().ToList();

            //add the most numerous digrams to remaining place
            int j = 0;
            int k = dictionary.Characters.Count;
            while (k < dictionary.Digrams.Count && k < dictionary.Capacity)
            {
                dictionary.AllSigns.Add($"{dictionary.Digrams[j].DigramValue}");
                dictionary.SignCodes.Add(Convert.ToString(k, 2).PadLeft(dictionary.BiteSize, '0'));
                j++;
                k++;
                // Console.WriteLine($"Digram:{dictionary.AllSigns[i]}     Code:{dictionary.SignCodes[i]}");
            }

            outputFilePath = $"../../../{filename}{selectedDictionarySize}.digram";
            DigramCode(dictionary, inputFilePath, outputFilePath);

            Console.WriteLine(@"Press any key to close program");
            Console.ReadLine();
        }
    }
}