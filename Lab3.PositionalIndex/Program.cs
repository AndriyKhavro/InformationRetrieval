using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InformationRetrieval.Common;

namespace Lab3.PositionalIndex
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var inputDirectory = args[0];

            var documents = DocumentProvider.GetDocuments(inputDirectory);

            var tokenizer = new WordDocumentTokenizer();

            var positionalIndex = new PositionalIndex(tokenizer);

            positionalIndex.AddDocuments(documents);
            
            while (true)
            {
                var input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                {
                    var resultDocs = positionalIndex.FindDocuments(input);
                    Console.WriteLine(string.Join("\n", resultDocs.Select(d => d.FilePath)));
                }
            }
        }
    }
}
