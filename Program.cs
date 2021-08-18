using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WordPuzzles
{
    class Program
    {
        static void Main(string[] args)
        {
            string dictFilename = "ordliste.txt";
            int wordMinLength = 7;
            int wordMaxLength = 10;

            // Get the project root directory.
            string cwd = Directory.GetCurrentDirectory();
            string projectPath = Directory.GetParent(cwd).Parent.Parent.FullName;

            // Create an absolute path to the dictionary file.
            string fileAbsPath = Path.Combine(projectPath, dictFilename);

            //string text = File.ReadAllText(fileAbsPath);

            string[] words = GetWords(fileAbsPath, wordMinLength, wordMaxLength);

            Random rng = new();

            int target = 200;

            for (int i = 0; i < target; i++)
            {
                string randomWord = words[rng.Next(0, words.Length)];

                string[] commonMatch = CommonThingie(randomWord, words);

                if (commonMatch != null)
                {
                    if (target > 200)
                    {
                        // If target has been raised, find the offset and
                        // print our indended match order, not current i.
                        int offset = target - 200;
                        Console.Write($"{i + 1 - offset}: ");
                    } else
                    {
                        Console.Write($"{i + 1}: ");
                    }
                    
                    PrintCommonMatch(commonMatch);
                    Console.WriteLine();
                } else
                {
                    // If not matched, increment target and continue.
                    target++;
                    continue;
                }
            }
            

        }

        private static void PrintCommonMatch(string[] commonMatch)
        {
            string w1 = commonMatch[0];
            string w2 = commonMatch[1];

            // Word 1
            for (int i = 0; i < w1.Length; i++)
            {
                if (i >= w1.Length - 3)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                }
                else
                {
                    Console.ResetColor();
                }

                Console.Write(w1[i]);
            }
            Console.ResetColor();
            Console.Write(" ");

            // Word 2
            for (int i = 0; i < w2.Length; i++)
            {
                if (i < 3)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                }
                else
                {
                    Console.ResetColor();
                }

                Console.Write(w2[i]);
            }
            Console.ResetColor();
        }

        private static string[] CommonThingie(string word, string[] words)
        {
            string common = word[^3..];

            foreach (string w in words)
            {
                if (w.StartsWith(common)) return new string[2] { word, w };
            }

            return null;
        }

        private static string[] GetWords(string fileAbsPath, int minLength, int maxLength)
        {
            List<string> words = new();

            string prevWord = string.Empty;
            foreach (string line in File.ReadLines(fileAbsPath, Encoding.UTF8))
            {
                string[] parts = line.Split('\t');
                string word = parts[1];

                if ( word.Contains('-') ) continue;

                // Only consider new words.
                if (word != prevWord && word.Length >= minLength && word.Length <= maxLength)
                {
                    words.Add(word);
                }

                prevWord = word;
            }

            return words.ToArray();
        }
    }
}