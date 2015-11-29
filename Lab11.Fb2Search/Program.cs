using System;
using System.Linq;
using InformationRetrieval.Common;

namespace Lab11.Fb2Search
{
    class Program
    {
        private static void Main(string[] args)
        {
            var directory = args[0];

            var documents = new Fb2ZoneDocumentProvider().GetDocuments(directory);

            var tokenizer = new WordDocumentTokenizer();

            var index = new InverceIndex<string, IDocument>(tokenizer);

            index.AddDocuments(documents);

            while (true)
            {
                var input = Console.ReadLine();
                var token = tokenizer.Tokenize(input).FirstOrDefault();
                if (token != null)
                {
                    Console.WriteLine(string.Join("\n",
                        index.GetDocumentsWithScore(token).Select(t => $"{t.Item1.FilePath}\tScore:{t.Item2}")));
                }
            }
        }
    }
}
