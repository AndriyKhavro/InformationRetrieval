using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Lab1
{
    internal class Program
    {
        private const string OUTPUT_FILE_PATH = "output.txt";

        private static void Main(string[] args)
        {
            var inputDirectoryPath = args[0];
            var outputPath = args.Length > 1 ? args[1] : OUTPUT_FILE_PATH;

            var files = Directory.GetFiles(inputDirectoryPath).Where(file => file.EndsWith(".txt")).ToArray();

            long collectionSize = files.Sum(file => new FileInfo(file).Length);

            var wordsInCollection = files.Select(File.ReadAllText)
                .SelectMany(GetWordsFromText).ToArray();

            int collectionCount = wordsInCollection.Count();

            var words =
                wordsInCollection
                    .Select(s => s.ToLower())
                    .Distinct()
                    .OrderBy(s => s)
                    .ToArray();

            int dictionaryCount = words.Count();

            File.WriteAllLines(outputPath, words);

            long dictionarySize = new FileInfo(outputPath).Length;

            Console.WriteLine(@"
Collection size:        {0} bytes;
Collection word count:  {1};
Dictionary size:        {2} bytes;
Dictionary word count:  {3};", collectionSize, collectionCount, dictionarySize, dictionaryCount);

            Console.ReadKey();
        }

        private static IEnumerable<string> GetWordsFromText(string text)
        {
            return Regex.Split(text, "[^a-zA-Z']").Where(s => !string.IsNullOrEmpty(s));
        }
    }
}
