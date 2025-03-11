using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace Text_Analyzer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine("╔══════════════════════════════════════╗");
            Console.WriteLine("( ͡◑ ‿‿ ͡◑) 👉 Въведете текст за анализ:");
            Console.WriteLine("╚══════════════════════════════════════╝");
            string inputText = Console.ReadLine();

           
           

            
            HashSet<string> stopWords = new HashSet<string> { "и", "на", "в", "с", "за", "да", "от", "се", "като", "по", "че", "не", "той", "които", "със", "тя", "те", "го", "му", "ги", "си", "тази", "тук", "там", "също", "са", "сме", "сте", "само", "още", "може", "би", "е" };

           
            int wordCount = CountWords(inputText);
            int charCount = CountCharacters(inputText);
            var wordFrequency = GetWordFrequency(inputText, stopWords);
            var sentenceStats = AnalyzeSentences(inputText);
            var punctuationStats = AnalyzePunctuation(inputText);
            string tense = DetectTense(inputText);

          
            Console.WriteLine($"\nСтатистика:");
            Console.WriteLine($"Брой думи: {wordCount}");
            Console.WriteLine($"Брой символи: {charCount}");
            Console.WriteLine($"Брой изречения: {sentenceStats.SentenceCount}");
            Console.WriteLine($"Средна дължина на изреченията: {sentenceStats.AverageSentenceLength:F2} думи");

            Console.WriteLine("\nЧесто срещани думи:");
            foreach (var pair in wordFrequency.OrderByDescending(p => p.Value).Take(10))
            {
                Console.WriteLine($"{pair.Key}: {pair.Value} пъти");
            }

            Console.WriteLine("\nУпотреба на препинателни знаци:");
            Console.WriteLine($"Точки (.): {punctuationStats['.']}");
            Console.WriteLine($"Запетаи (,): {punctuationStats[',']}");
            Console.WriteLine($"Въпросителни знаци (?): {punctuationStats['?']}");
            Console.WriteLine($"Удивителни знаци (!): {punctuationStats['!']}");
            Console.WriteLine($"Многоточия (…): {punctuationStats['…']}");
            Console.WriteLine($"Точка и запетая (;): {punctuationStats[';']}");
            Console.WriteLine($"Двоеточия (:): {punctuationStats[':']}");
            Console.WriteLine($"Тире (–): {punctuationStats['–']}");
            Console.WriteLine($"Дефис (-): {punctuationStats['-']}");
            Console.WriteLine($"Кавички (“”): {punctuationStats['“']}");
            Console.WriteLine($"Скоби (()): {punctuationStats['(']}");

            Console.WriteLine($"\nВреме на текста: {tense}");
        }

        
        static string DetectScriptType(string text)
        {
         
            bool containsCyrillic = text.Any(c => c >= 'А' && c <= 'я');

            if (containsCyrillic)
            {
                return "Кирилица";
            }
            else
            {
                return "Латиница";
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
            
            Dictionary<char, int> punctuationStats = new Dictionary<char, int>
        {
            { '.', 0 },
            { ',', 0 },
            { '?', 0 },
            { '!', 0 },
            { '…', 0 },
            { ';', 0 },
            { ':', 0 },
            { '–', 0 },
            { '-', 0 },
            { '“', 0 },
            { '”', 0 },
            { '(', 0 },
            { ')', 0 }
        };

           
            foreach (char c in text)
            {
                if (punctuationStats.ContainsKey(c))
                {
                    punctuationStats[c]++;
                }
            }

           
            punctuationStats['…'] = text.Split(new[] { "…" }, StringSplitOptions.None).Length - 1;

            return punctuationStats;
        }

    
        static string DetectTense(string text)
        {
           
            HashSet<string> presentTenseIndicators = new HashSet<string> { "съм", "е", "сме", "сте", "са", "мога", "искам", "правя", "ходя", "пиша", "чета" };
            HashSet<string> pastIndefiniteIndicators = new HashSet<string> { "бях", "беше", "бяхме", "бяхте", "бяха", "можех", "исках", "правих", "ходих", "писах", "четох" };
            HashSet<string> pastImperfectIndicators = new HashSet<string> { "бях", "беше", "бяхме", "бяхте", "бяха", "можеше", "искаше", "правеше", "ходеше", "пишеше", "четяше" };
            HashSet<string> pastPerfectIndicators = new HashSet<string> { "бях", "беше", "бяхме", "бяхте", "бяха", "бил съм", "бил е", "били сме", "били сте", "били са", "можел съм", "искал съм", "правил съм", "ходил съм", "писал съм", "чел съм" };
            HashSet<string> futureTenseIndicators = new HashSet<string> { "ще", "ще бъда", "ще бъде", "ще бъдем", "ще бъдете", "ще бъдат", "ще мога", "ще искам", "ще правя", "ще ходя", "ще пиша", "ще чета" };
            HashSet<string> futureInThePastIndicators = new HashSet<string> { "щях", "щеше", "щяхме", "щяхте", "щяха", "щях да", "щеше да", "щяхме да", "щяхте да", "щяха да" };

          
            string[] words = text.Split(new[] { ' ', '\t', '\n', '\r', '.', ',', '!', '?', ';', ':', '(', ')' }, StringSplitOptions.RemoveEmptyEntries);

          
            bool hasPresentTense = words.Any(word => presentTenseIndicators.Contains(word.ToLower()));
            bool hasPastIndefiniteTense = words.Any(word => pastIndefiniteIndicators.Contains(word.ToLower()));
            bool hasPastImperfectTense = words.Any(word => pastImperfectIndicators.Contains(word.ToLower()));
            bool hasPastPerfectTense = words.Any(word => pastPerfectIndicators.Contains(word.ToLower()));
            bool hasFutureTense = words.Any(word => futureTenseIndicators.Contains(word.ToLower()));
            bool hasFutureInThePastTense = words.Any(word => futureInThePastIndicators.Contains(word.ToLower()));

           
            if (hasFutureInThePastTense)
            {
                return "Бъдеще време в миналото";
            }
            else if (hasFutureTense)
            {
                return "Бъдеще време";
            }
            else if (hasPastPerfectTense)
            {
                return "Минало предварително време";
            }
            else if (hasPastImperfectTense)
            {
                return "Минало несвършено време";
            }
            else if (hasPastIndefiniteTense)
            {
                return "Минало неопределено време";
            }
            else if (hasPresentTense)
            {
                return "Сегашно време";
            }
            else
            {
                return "Неопределено време";
            }
        }
    }
}
