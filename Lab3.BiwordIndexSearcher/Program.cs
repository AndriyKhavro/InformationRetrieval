using System;
using System.Linq;
using InformationRetrieval.Common;
using Lab3.BiwordIndex;

namespace Lab3.BiwordIndexSearcher
{
    class Program
    {
        private static void Main(string[] args)
        {
            var inputDirectory = args[0];

            var documents = DocumentProvider.GetDocuments(inputDirectory);

            var tokenizer = new PhraseDocumentTokenizer(new PartOfSpeechTagger());

            var inverseIndex = new InverceIndex<Phrase>(tokenizer);

            inverseIndex.AddDocuments(documents);

            var searcher = new PhraseQueryExecutor(inverseIndex);

            while (true)
            {
                var input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                {
                    var resultDocs = searcher.ProcessQuery(input);
                    Console.WriteLine(string.Join("\n", resultDocs.Select(d => d.FilePath)));
                }
            }
        }
    }
}
