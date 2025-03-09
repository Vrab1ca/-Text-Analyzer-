using System;
using System.Collections.Generic;
using System.Linq;

namespace Text_Analyzer
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("╔══════════════════════════════════════╗");
            Console.WriteLine("VAVEDETE TEKST ZA ANALIZ");
            Console.WriteLine("╚══════════════════════════════════════╝");
            string inputText = Console.ReadLine();

         
            HashSet<string> stopWords = new HashSet<string> { "и", "на", "в", "с", "за", "да", "от", "се", "като", "по", "че", "не", "той", "които", "със", "тя", "те", "го", "му", "ги", "си", "тази", "тук", "там", "също", "са", "сме", "сте", "само", "още", "може", "би", "е" };

    
            int wordCount = CountWords(inputText);
            int charCount = CountCharacters(inputText);
            var wordFrequency = GetWordFrequency(inputText, stopWords);
            var sentenceStats = AnalyzeSentences(inputText);
            var punctuationStats = AnalyzePunctuation(inputText);


            Console.WriteLine($"\nStatistika:");
            Console.WriteLine($"Broi dumi: {wordCount}");
            Console.WriteLine($"Broi simvoli: {charCount}");
            Console.WriteLine($"Broi izrecheniq: {sentenceStats.SentenceCount}");
            Console.WriteLine($"Sredna dalzhina na izrecheniqta: {sentenceStats.AverageSentenceLength:F2} думи");

            Console.WriteLine("\nChesto sreshtani dumi:");
            foreach (var pair in wordFrequency.OrderByDescending(p => p.Value).Take(10))
            {
                Console.WriteLine($"{pair.Key}: {pair.Value} pati");
            }

            Console.WriteLine("\nUpotreba na prepinatelni znaci:");
            foreach (var pair in punctuationStats)
            {
                Console.WriteLine($"{pair.Key}: {pair.Value} pati");
            }
        }


        static int CountWords(string text)
        {
            string[] words = text.Split(new[] { ' ', '\t', '\n', '\r', '.', ',', '!', '?', ';', ':', '(', ')' }, StringSplitOptions.RemoveEmptyEntries);
            return words.Length;
        }


        static int CountCharacters(string text)
        {
            return text.Replace(" ", "").Length;
        }


        static Dictionary<string, int> GetWordFrequency(string text, HashSet<string> stopWords)
        {
            string[] words = text.Split(new[] { ' ', '\t', '\n', '\r', '.', ',', '!', '?', ';', ':', '(', ')' }, StringSplitOptions.RemoveEmptyEntries);
            Dictionary<string, int> frequency = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            foreach (string word in words)
            {
                string cleanedWord = word.Trim().ToLower();
                if (!stopWords.Contains(cleanedWord))
                {
                    if (frequency.ContainsKey(cleanedWord))
                    {
                        frequency[cleanedWord]++;
                    }
                    else
                    {
                        frequency[cleanedWord] = 1;
                    }
                }
            }

            return frequency;
        }

        // Analyze sentence length and count
        static (int SentenceCount, double AverageSentenceLength) AnalyzeSentences(string text)
        {
            char[] sentenceSeparators = { '.', '!', '?' };
            string[] sentences = text.Split(sentenceSeparators, StringSplitOptions.RemoveEmptyEntries);
            int sentenceCount = sentences.Length;
            int totalWordsInSentences = 0;

            foreach (string sentence in sentences)
            {
                totalWordsInSentences += CountWords(sentence);
            }

            double averageSentenceLength = sentenceCount > 0 ? (double)totalWordsInSentences / sentenceCount : 0;
            return (sentenceCount, averageSentenceLength);
        }


        static Dictionary<char, int> AnalyzePunctuation(string text)
        {
            char[] punctuationMarks = { '.', ',', '!', '?', ';', ':', '(', ')' };
            Dictionary<char, int> punctuationStats = new Dictionary<char, int>();

            foreach (char mark in punctuationMarks)
            {
                int count = text.Count(c => c == mark);
                if (count > 0)
                {
                    punctuationStats[mark] = count;
                }
            }

            return punctuationStats;
        }
    }
}
