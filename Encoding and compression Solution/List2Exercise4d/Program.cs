using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace List2Exercise4d
{
    internal class Myletter
    {
        public Myletter(char letter)
        {
            this.Letter = letter;
            this.Quantity = 1;
        }

        public char Letter { get; set; }
        public int Quantity { get; set; }
        public double Probability { get; set; }
    }

    internal class Nextletter : Myletter
    {
        public Nextletter(char letter, char contextLetter)
            : base(letter)
        {
            this.ContextLetter = contextLetter;
        }

        public Nextletter(char letter, string contextString)
            : base(letter)
        {
            this.ContextString = contextString;
        }

        public char ContextLetter { get; set; }
        public string ContextString { get; set; }
    }

    internal class Program
    {
        private static List<char[]> GetWords(string Path)
        {
            List<char[]> Words = new List<char[]>();
            StreamReader reader = new StreamReader(Path);
            while (!reader.EndOfStream)
            {
                char[] word = new char[4];
                for (int i = 0; i < 4; i++)
                {
                    word[i] = (char)reader.Read();
                }

                reader.Read();
                reader.Read();
                Words.Add(word);
            }
            return Words;
        }

        private static List<Myletter> PrepareFirstLetters(List<char[]> words)
        {
            List<Myletter> firstLetters = new List<Myletter>();
            foreach (char[] word in words)
            {
                char firstCharacter = word[0];

                int index = firstLetters.FindIndex(x => x.Letter == firstCharacter);

                if (index != -1) firstLetters[index].Quantity++;
                else firstLetters.Add(new Myletter(firstCharacter));
            }

            CalculateProbability(firstLetters);

            foreach (Myletter myLetter in firstLetters)
            {
                Console.WriteLine($"Letter-{myLetter.Letter}  Quantity-{myLetter.Quantity}  Probability-{myLetter.Probability}");
            }
            return firstLetters;
        }

        private static List<Nextletter> PrepareNextLetters(List<char[]> words, int posNum)
        {
            List<Nextletter> nextLetters = new List<Nextletter>();
            foreach (char[] word in words)
            {
                char nextCharacter = word[posNum];
                char contextCharacter = word[posNum - 1];

                int index = nextLetters.FindIndex(x => x.Letter == nextCharacter && x.ContextLetter == contextCharacter);

                if (index != -1) nextLetters[index].Quantity++;
                else nextLetters.Add(new Nextletter(nextCharacter, contextCharacter));
            }
            nextLetters = nextLetters.OrderBy(x => x.ContextLetter).ThenBy(x => x.Letter).ToList();

            CalculateProbability(nextLetters);

            foreach (Nextletter nextletter in nextLetters)
            {
                Console.WriteLine($"Context-{nextletter.ContextLetter}  Letter-{nextletter.Letter}  Quantity-{nextletter.Quantity}  Probability-{nextletter.Probability}");
            }
            return nextLetters;
        }

        private static List<Nextletter> PrepareNextLetters(List<char[]> words, int posNum, int contextsize)
        {
            List<Nextletter> nextLetters = new List<Nextletter>();
            foreach (char[] word in words)
            {
                char nextCharacter = word[posNum];
                string contextString = "";
                for (int i = 0; i < contextsize; i++)
                {
                    contextString += word[posNum - contextsize + i];
                }

                int index = nextLetters.FindIndex(x => x.Letter == nextCharacter && x.ContextString == contextString);

                if (index != -1) nextLetters[index].Quantity++;
                else nextLetters.Add(new Nextletter(nextCharacter, contextString));
            }
            nextLetters = nextLetters.OrderBy(x => x.ContextString).ThenBy(x => x.Letter).ToList();

            CalculateProbability(nextLetters, contextsize);

            foreach (Nextletter nextletter in nextLetters)
            {
                Console.WriteLine($"Context-{nextletter.ContextString}  Letter-{nextletter.Letter}  Quantity-{nextletter.Quantity}  Probability-{nextletter.Probability}");
            }
            return nextLetters;
        }

        private static void CalculateProbability(List<Myletter> letters)
        {
            int sum = letters.Sum(x => x.Quantity);
            foreach (Myletter myletter in letters)
            {
                myletter.Probability = (double)myletter.Quantity / sum;
            }
        }

        private static void CalculateProbability(List<Nextletter> letters)
        {
            List<char> contexts = new List<char>();
            foreach (Nextletter nextletter in letters)
            {
                contexts.Add(nextletter.ContextLetter);
            }
            contexts = contexts.Distinct().ToList();

            foreach (char context in contexts)
            {
                int sum = letters.Where(x => x.ContextLetter == context).Sum(x => x.Quantity);
                foreach (Nextletter nextletter in letters)
                {
                    if (nextletter.ContextLetter == context) nextletter.Probability = (double)nextletter.Quantity / sum;
                }
            }
        }

        private static void CalculateProbability(List<Nextletter> letters, int contextSize)
        {
            List<string> contexts = new List<string>();
            foreach (Nextletter nextletter in letters)
            {
                contexts.Add(nextletter.ContextString);
            }
            contexts = contexts.Distinct().ToList();

            foreach (string context in contexts)
            {
                int sum = letters.Where(x => x.ContextString == context).Sum(x => x.Quantity);
                foreach (Nextletter nextletter in letters)
                {
                    if (nextletter.ContextString == context) nextletter.Probability = (double)nextletter.Quantity / sum;
                }
            }
        }

        private static char SelectLetter(List<Myletter> letters, Random rand)
        {
            return WeightedChoice(letters, rand);
        }

        private static char SelectLetter(List<Nextletter> letters, Random rand, char context)
        {
            List<Nextletter> lettersWithContext = letters.Where(x => x.ContextLetter == context).ToList();
            return WeightedChoice(lettersWithContext.Cast<Myletter>().ToList(), rand);
        }

        private static char SelectLetter(List<Nextletter> letters, Random rand, char context1, char context2)
        {
            List<Nextletter> lettersWithContext = letters.Where(x => x.ContextString == $"{context1}{context2}").ToList();
            return WeightedChoice(lettersWithContext.Cast<Myletter>().ToList(), rand);
        }

        private static char WeightedChoice(List<Myletter> letters, Random rand)
        {
            double sum = 0;
            double r = rand.NextDouble();
            foreach (Myletter myletter in letters)
            {
                if (r < (sum + myletter.Probability))
                {
                    return myletter.Letter;
                }
                else
                {
                    sum += myletter.Probability;
                }
            }
            return '\x0000';
        }

        public static void ClearCurrentConsoleLine()
        {
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }

        private static void Main(string[] args)
        {
            Random rand = new Random();
            string filePath = "../../../4wyrazy.txt";
            List<char[]> Words = GetWords(filePath);

            StringBuilder stringBuilder = new StringBuilder();

            Console.WriteLine("First Letter");
            List<Myletter> FirstLetters = PrepareFirstLetters(Words);
            Console.WriteLine("Second Letter");
            List<Nextletter> SecondLetters = PrepareNextLetters(Words, 1);
            Console.WriteLine("Third Letter");
            List<Nextletter> ThirdLetters = PrepareNextLetters(Words, 2, 2);
            Console.WriteLine("Fourth Letter");
            List<Nextletter> FourthLetters = PrepareNextLetters(Words, 3, 2);

            for (int i = 0; i < 200; i++)
            {
                Console.WriteLine($"Creating {i} word");

                char first = SelectLetter(FirstLetters, rand);
                char second = SelectLetter(SecondLetters, rand, first);
                char third = SelectLetter(ThirdLetters, rand, first, second);
                char fourth = SelectLetter(FourthLetters, rand, second, third);
                stringBuilder.Append($"{first}{second}{third}{fourth}\r\n");
                Thread.Sleep(30);
                ClearCurrentConsoleLine();
            }
            Console.WriteLine("Words Created");

            Thread.Sleep(500);
            File.WriteAllText(@"ProbabilityModelWith2Context.txt", stringBuilder.ToString());
            Console.WriteLine("Created file 'ProbabilityModelWith2Context.txt'");
            Console.WriteLine("Click any key to go to project folder...");
            Console.ReadKey();

            System.Diagnostics.Process.Start("explorer.exe", Directory.GetCurrentDirectory());

            Console.WriteLine("Press any key to close program");
            Console.ReadLine();
        }
    }
}