using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InformationRetrieval.Common;

namespace Lab2.BooleanSearcher
{
    public class BooleanSearchProgram
    {
        private readonly string _inputDirectory;
        private readonly IDocumentProvider _documentProvider;

        public BooleanSearchProgram(string inputDirectory, IDocumentProvider documentProvider)
        {
            _inputDirectory = inputDirectory;
            _documentProvider = documentProvider;
        }

        public void Launch()
        {
            var documents = _documentProvider.GetDocuments(_inputDirectory);
            var tokenizer = new WordDocumentTokenizer();

            var inverseIndex = new InverceIndex<string, IDocument>(tokenizer);

            inverseIndex.AddDocuments(documents);

            var searcher = new InformationRetrieval.Common.BooleanSearcher(inverseIndex);

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
