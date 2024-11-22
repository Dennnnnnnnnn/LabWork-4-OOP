// Text file L3_3T.txt contains text. The delimiters are known.
// Text in each line is read from left to right.
// Move each word that is a palindrome to the beginning of line
// with delimiters after it. Write modified text to a separate
// result file Results.txt. Prepare analysis file Analysis.txt
// using table format (2 columns): line number, moved palindrome word.

using System;
using System.Text;
using System.IO;
namespace LW3
{
    /// <summary>
    /// Primary class to execute required calculations tasks 
    /// </summary>
    internal class Program
    {
        const string filename = "Result.txt"; // Result file name
        const string data = "L3_3T.txt"; // Data file name
        const string analysis = "Analysis.txt"; // Analysis file name
        static void Main(string[] args)
        {
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
            if (File.Exists(analysis))
            {
                File.Delete(analysis);
            }
            // Word delimiters
            char[] delimiters =
                { ' ', '.', ',', '!', '?', ':', ';', '(', ')', '\t' };
            Process(filename, data, analysis, delimiters);
        }
        /// <summary>
        /// Processes the input text, moves palindromes to the 
        /// beginning of each line and writes results to 
        /// result and analysis files.
        /// </summary>
        /// <param name="result"> Result file name</param>
        /// <param name="data"> Data file name</param>
        /// <param name="analysis">Analysis file name</param>
        /// <param name="delimiters"> Word delimiters</param>
        static void Process(string result, string data,
            string analysis, char[] delimiters)
        {
            using (StreamReader reader = new StreamReader(data, Encoding.UTF8))
            {
                string line; // Line of the text
                int numberOfLine = 0; // Number of line
                int wordCounter = 0; // Position of word in line
                bool header = true; //Indicates if header is needed
                while ((line = reader.ReadLine()) != null)
                {
                    numberOfLine++;
                    if (!string.IsNullOrEmpty(line))
                    {
                        // Words divided by delimiters
                        string[] words = line.Split(delimiters,
                            StringSplitOptions.RemoveEmptyEntries);
                        // Positions of previous palindrome
                        int pos = 0;
                        for (int i = 0; i < words.Length; i++)
                        {
                            string word = words[i]; // Current word
                            if (Palindrome(word))
                            {
                                line = MovePalindromeToBeginning
                                    (line, words, ref pos, i);
                                PrintAnalysis(analysis,
                                    numberOfLine, word, header);
                                header = false;
                            }
                            else
                            {
                                string wordWithDelimiters =
                                 WordWithDelimiters(line, words, ref pos, i);
                            }
                            wordCounter++;
                        }
                    }
                    Print(filename, line);
                }
            }
        }
        /// <summary>
        /// Method to print data to result file
        /// </summary>
        /// <param name="filename"> Result file name</param>
        /// <param name="line"> Current line</param>
        static void Print(string filename, string line)
        {
            using (StreamWriter writer = new StreamWriter(filename, true))
            {
                writer.WriteLine(line);
            }
        }
        /// <summary>
        /// Method to print data to analysis file
        /// </summary>
        /// <param name="analysis"> Analysis file name</param>
        /// <param name="numberOfLine"> Current number of line</param>
        /// <param name="word"> Moved palindrome</param>
        /// <param name="header"> Indicates if header is needed</param>
        static void PrintAnalysis(string analysis, int numberOfLine,
            string word, bool header)
        {
            using (StreamWriter an = new StreamWriter(analysis, true))
            {
                if (header)
                {
                    an.WriteLine("Line number  | Moved palindrome");
                    an.WriteLine("-------------------------------");
                }
                an.WriteLine("{0, -12} | {1, -15}", numberOfLine, word);
            }
        }
        /// <summary>
        /// Checks if the word is palindrome
        /// </summary>
        /// <param name="word"> Word in line</param>
        /// <returns> True if the word is palindrome, false otherwise</returns>
        static bool Palindrome(string word)
        {
            for (int i = 0; i < (word.Length / 2); i++)
            {
                if (Char.ToLower(word[i]) ==
                    Char.ToLower(word[word.Length - 1 - i]))
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// Moves palindrome to beginning of line
        /// </summary>
        /// <param name="line"> Line of text</param>
        /// <param name="words"> Words divided by delimiters</param>
        /// <param name="position"> Position of previous palindrome</param>
        /// <param name="i"> Position of palindrome</param>
        /// <returns> Modified line</returns>
        static string MovePalindromeToBeginning(string line,
            string[] words, ref int position, int i)
        {
            // Palindrome with delimiters
            string palindrome = PalindromeWithDelimiters
                (ref line, words, ref position, i);
            line = line.Insert(0, palindrome);
            return line;
        }
        /// <summary>
        /// Substrings palindrome with delimiters after it
        /// </summary>
        /// <param name="line"> Line in text</param>
        /// <param name="words"> Words divided by delimiters</param>
        /// <param name="position"> Position of previous palindrome</param>
        /// <param name="i"> Position of palindrome</param>
        /// <returns> Palindrome with delimiters</returns>
        static string PalindromeWithDelimiters(ref string line,
            string[] words, ref int position, int i)
        {
            // Length of palindrome with delimiters
            int lengthOfPalindromeWithDelimiters = -1;
            // Position of palindrome in line
            int positionOfPalindrome = line.IndexOf(words[i], position);
            // Position of word after palindrome in line
            int indWordAfterPal;

            if (i + 1 < words.Length)
            {
                indWordAfterPal = line.IndexOf(words[i + 1]);
            }
            else
            {
                indWordAfterPal = line.Length;
            }

            lengthOfPalindromeWithDelimiters =
                indWordAfterPal - positionOfPalindrome;
            // Palindrome with delimiters
            string palindromeWithDelimiters = line.Substring
                (positionOfPalindrome, lengthOfPalindromeWithDelimiters);
            line = line.Remove
                (positionOfPalindrome, lengthOfPalindromeWithDelimiters);
            position += palindromeWithDelimiters.Length;

            return palindromeWithDelimiters;
        }
        /// <summary>
        /// Substrings word with delimiters after it
        /// </summary>
        /// <param name="line"> Line in text</param>
        /// <param name="words"> Words divided by delimiters</param>
        /// <param name="position"> Position of previous word</param>
        /// <param name="i"> Position of word</param>
        /// <returns> Word with delimiters</returns>
        static string WordWithDelimiters(string line,
            string[] words, ref int position, int i)
        {
            // Length of word with delimiters
            int lengthOfWordWithDelimiters = -1;
            // Position of word in line
            int positionOfWord = line.IndexOf(words[i], position);
            // Next word index
            int indWord1;

            if (i + 1 < words.Length)
            {
                indWord1 = line.IndexOf
                    (words[i + 1], position + words[i].Length);
            }
            else
            {
                indWord1 = line.Length;
            }

            lengthOfWordWithDelimiters =
                indWord1 - positionOfWord;
            // Word with delimiters
            string wordWithDelimiters = line.Substring
                (positionOfWord, lengthOfWordWithDelimiters);

            position += wordWithDelimiters.Length;

            return wordWithDelimiters;
        }
    }
}