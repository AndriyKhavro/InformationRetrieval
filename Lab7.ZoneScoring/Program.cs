using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InformationRetrieval.Common;
using Lab2.BooleanSearcher;

namespace Lab7.ZoneScoring
{
    class Program
    {
        private static void Main(string[] args)
        {
            var directory = args[0];

            var documents = new ZoneDocumentProvider().GetDocuments(directory);

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
