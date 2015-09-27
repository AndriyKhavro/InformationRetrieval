﻿using System;
using System.Linq;
using Lab2.InverseIndex;

namespace Lab2.BooleanSearcher
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var inputDirectory = args[0];

            var documents = DocumentProvider.GetDocuments(inputDirectory);

            var tokenizer = new WordDocumentTokenizer();

            var inverseIndex = new InverceIndex<string>(tokenizer);

            inverseIndex.AddDocuments(documents);

            var searcher = new InverseIndex.BooleanSearcher(inverseIndex);

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
