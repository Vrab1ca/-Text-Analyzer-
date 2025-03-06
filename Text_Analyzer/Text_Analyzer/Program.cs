using System:
using System.Collections.Generic;
using System.Linq;

namespace Text_Analyzer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("╔══════════════════════════════════════╗");
            Console.WriteLine("(っ◔◡◔)っ ♥ Въведете текст за анализ ♥");
            Console.WriteLine("╚══════════════════════════════════════╝");
            string inputText = Console.ReadLine();
            HashSet<string> stopWords = new Hashet<string> 
            { "и" , "на" , "в" , "с" , "за" , "да" , "от" , "се" , "като" , "по" ,
            "че" , "не" , "той" , "които" , "със" , "тя" , "те" , "го" , "му" , "ги" ,
             "си" , "тази" , "тук" , "там" , "също" , "са" , "сме" , "сте" , "само" , "още" ,
             "може" , "би" , "е" };

          int wordCount = CountWords(inputText);
          int charCount = CountCharacters(inputText);
          var wordFrequency = GetWordFrequency(inputText , stopWords);

          Console.WriteLine($"\nСтатистика : ");
          Console.Write
        }
    }
}
