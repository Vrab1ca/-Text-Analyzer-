using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Text_Analyzer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Set the console encoding to UTF-8 to support Bulgarian characters
            Console.OutputEncoding = Encoding.UTF8;

            while (true) // Loop to keep the program running until the user exits
            {
                // Display the input prompt with the exit button in a box
                Console.WriteLine("╔═════════════════════════════════════════════════════╗");
                Console.WriteLine("(◔◡◔)✍ Въведете текст (или 'Ext' за изход): ☜(ˆ▿ˆc) ");
                Console.WriteLine("╚═════════════════════════════════════════════════════╝");

                string inputText = Console.ReadLine();

                // Check if the user wants to exit
                if (inputText.Trim().Equals("Ext", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Излизане от програмата...");
                    break; // Exit the loop and end the program
                }

                // Common Bulgarian stop words to ignore
                HashSet<string> stopWords = new HashSet<string> { "и", "на", "в", "с", "за", "да", "от", "се", "като", "по", "че", "не", "той", "които", "със", "тя", "те", "го", "му", "ги", "си", "тази", "тук", "там", "също", "са", "сме", "сте", "само", "още", "може", "би", "е" };

                // Analyze the text
                int wordCount = CountWords(inputText);
                int charCount = CountCharacters(inputText);
                var wordFrequency = GetWordFrequency(inputText, stopWords);
                var sentenceStats = AnalyzeSentences(inputText);
                var punctuationStats = AnalyzePunctuation(inputText);
                string tense = DetectTense(inputText);

                // Display results
                Console.WriteLine($"\n▒Брой думи▒: {wordCount}");
                Console.WriteLine($"▙Брой символи▜: {charCount}");
                Console.WriteLine($"▬Брой изречения▬: {sentenceStats.SentenceCount}");
                Console.WriteLine($"⌘Средна дължина на изреченията⌘: {sentenceStats.AverageSentenceLength:F2} думи");

                Console.WriteLine("\n☵Често срещани думи☵:");
                var topWords = wordFrequency.OrderByDescending(p => p.Value).Take(10);
                foreach (var pair in topWords)
                {
                    Console.WriteLine($"{pair.Key}: {pair.Value} пъти");
                }

                Console.WriteLine("\n▣Употреба на препинателни знаци▣:");
                Console.WriteLine($"⌑Точки (.)⌑: {punctuationStats['.']}");
                Console.WriteLine($"▪Запетаи (,)▪: {punctuationStats[',']}");
                Console.WriteLine($"▩Въпросителни знаци (?)▩: {punctuationStats['?']}");
                Console.WriteLine($"◎Удивителни знаци (!)◎: {punctuationStats['!']}");
                Console.WriteLine($"○Многоточия (…)○: {punctuationStats['…']}");
                Console.WriteLine($"┣Точка и запетая (;)┨: {punctuationStats[';']}");
                Console.WriteLine($"╍Двоеточия (:)╍: {punctuationStats[':']}");
                Console.WriteLine($"╞Тире (–)╡: {punctuationStats['–']}");
                Console.WriteLine($"╏Дефис (-)╏: {punctuationStats['-']}");
                Console.WriteLine($"🔹Кавички (“”)🔹: {punctuationStats['“']}");
                Console.WriteLine($"▦Скоби (())▦: {punctuationStats['(']}");

                Console.WriteLine($"\n❒Време на текста: {tense}");

                // Add a separator for better readability
                Console.WriteLine("\n════════════════════════════════════════\n");
            }
        }

        // Count the total number of words
        static int CountWords(string text)
        {
            // Split text into words using spaces and punctuation as separators
            string[] words = text.Split(new[] { ' ', '\t', '\n', '\r', '.', ',', '!', '?', ';', ':', '(', ')' }, StringSplitOptions.RemoveEmptyEntries);
            return words.Length;
        }

        // Count the total number of characters (excluding spaces)
        static int CountCharacters(string text)
        {
            return text.Replace(" ", "").Length;
        }

        // Get the frequency of each word, ignoring stop words
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
            // Split text into sentences using '.', '!', and '?' as separators
            string[] sentences = text.Split(new[] { '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);
            int sentenceCount = sentences.Length;
            int totalWordsInSentences = 0;

            foreach (string sentence in sentences)
            {
                totalWordsInSentences += CountWords(sentence);
            }

            double averageSentenceLength = sentenceCount > 0 ? (double)totalWordsInSentences / sentenceCount : 0;
            return (sentenceCount, averageSentenceLength);
        }

        // Analyze punctuation usage
        static Dictionary<char, int> AnalyzePunctuation(string text)
        {
            // Initialize dictionary with all punctuation marks
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

            // Count each punctuation mark
            foreach (char c in text)
            {
                if (punctuationStats.ContainsKey(c))
                {
                    punctuationStats[c]++;
                }
            }

            // Handle ellipses (three consecutive dots)
            punctuationStats['…'] = text.Split(new[] { "…" }, StringSplitOptions.None).Length - 1;

            return punctuationStats;
        }

        // Detect the tense of the text
        static string DetectTense(string text)
        {
            // Common Bulgarian verb forms and tense indicators
            HashSet<string> presentTenseIndicators = new HashSet<string> { "съм", "е", "сме", "сте", "са", "мога", "искам", "правя", "ходя", "пиша", "чета" };
            HashSet<string> pastIndefiniteIndicators = new HashSet<string> { "бях", "беше", "бяхме", "бяхте", "бяха", "можех", "исках", "правих", "ходих", "писах", "четох" };
            HashSet<string> pastImperfectIndicators = new HashSet<string> { "бях", "беше", "бяхме", "бяхте", "бяха", "можеше", "искаше", "правеше", "ходеше", "пишеше", "четяше" };
            HashSet<string> pastPerfectIndicators = new HashSet<string> { "бях", "беше", "бяхме", "бяхте", "бяха", "бил съм", "бил е", "били сме", "били сте", "били са", "можел съм", "искал съм", "правил съм", "ходил съм", "писал съм", "чел съм" };
            HashSet<string> futureTenseIndicators = new HashSet<string> { "ще", "ще бъда", "ще бъде", "ще бъдем", "ще бъдете", "ще бъдат", "ще мога", "ще искам", "ще правя", "ще ходя", "ще пиша", "ще чета" };
            HashSet<string> futureInThePastIndicators = new HashSet<string> { "щях", "щеше", "щяхме", "щяхте", "щяха", "щях да", "щеше да", "щяхме да", "щяхте да", "щяха да" };

            // Split the text into words
            string[] words = text.Split(new[] { ' ', '\t', '\n', '\r', '.', ',', '!', '?', ';', ':', '(', ')' }, StringSplitOptions.RemoveEmptyEntries);

            // Check for tense indicators
            bool hasPresentTense = words.Any(word => presentTenseIndicators.Contains(word.ToLower()));
            bool hasPastIndefiniteTense = words.Any(word => pastIndefiniteIndicators.Contains(word.ToLower()));
            bool hasPastImperfectTense = words.Any(word => pastImperfectIndicators.Contains(word.ToLower()));
            bool hasPastPerfectTense = words.Any(word => pastPerfectIndicators.Contains(word.ToLower()));
            bool hasFutureTense = words.Any(word => futureTenseIndicators.Contains(word.ToLower()));
            bool hasFutureInThePastTense = words.Any(word => futureInThePastIndicators.Contains(word.ToLower()));

            // Determine the tense
            if (hasFutureInThePastTense)
            {
                return "▻Бъдеще време в миналото◅";
            }
            else if (hasFutureTense)
            {
                return "►Бъдеще време◄";
            }
            else if (hasPastPerfectTense)
            {
                return "▷Минало предварително време◁";
            }
            else if (hasPastImperfectTense)
            {
                return "◣Минало несвършено време◢";
            }
            else if (hasPastIndefiniteTense)
            {
                return "▀Минало неопределено време▀";
            }
            else if (hasPresentTense)
            {
                return "┏Сегашно време┓";
            }
            else
            {
                return "◌Неопределено време◌";
            }
        }
    }
}
