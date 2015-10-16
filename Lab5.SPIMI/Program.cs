using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using InformationRetrieval.Common;
using Lab6.IndexCompression;

namespace Lab5.SPIMI
{
    internal class Program
    {
        private const int TermsInBlock = 10000;
        private const string InputDir = @"D:\univ\files";
        private const string OutputDir = @"D:\univ\output";

        private static void Main(string[] args)
        {
            var docs = DocumentProvider.GetDocuments(InputDir);

            var indexSerializer = IndexSerializerFactory.Create(Compression.No);

            var termBlocks = GetTermBlocks(docs, new DocumentIdTracker(OutputDir));

            int i = 1;

            //we write all terms dictionaries in files
            foreach (var termBlock in termBlocks)
            {
                indexSerializer.SerializeToFile(Path.Combine(OutputDir, $"{i++}.txt"), termBlock);
            }

            //and then we just need to merge it simulteneously reading from all created files
            var fileNames = Enumerable.Range(1, i-1).Select(num => Path.Combine(OutputDir, $"{num}.txt")).ToArray();


            var mergedPairs = MergeBlocks(fileNames.Select(file => indexSerializer.DeserializeByLine(file)));

            indexSerializer.SerializeToFileByLine(Path.Combine(OutputDir, "output.txt"), mergedPairs);

            foreach (var fileName in fileNames)
            {
                File.Delete(fileName);
            }
            Console.WriteLine("End");
            Console.ReadKey();
        }

        private static IEnumerable<KeyValuePair<string, HashSet<int>>> MergeBlocks(IEnumerable<IEnumerable<KeyValuePair<string, HashSet<int>>>> blocks)
        {
            var enumerators =
                blocks.Select(b => b.GetEnumerator()).Where(e => e.MoveNext()).OrderBy(e => e.Current.Key).ToArray();
            while (enumerators.Any())
            {
                var firstEnumerator = enumerators.First();
                string currentTerm = firstEnumerator.Current.Key;
                var currentSet = new HashSet<int>(firstEnumerator.Current.Value);
                if (enumerators.Length == 1)
                {
                    yield return new KeyValuePair<string, HashSet<int>>(currentTerm, currentSet);
                    if (!firstEnumerator.MoveNext())
                    {
                        yield break;
                    }
                }
                else
                {
                    int i = 1;
                    for (; i < enumerators.Length; i++)
                    {
                        if (currentTerm == enumerators[i].Current.Key)
                        {
                            currentSet.UnionWith(enumerators[i].Current.Value);
                        }

                        else
                        {
                            break;
                        }
                    }

                    yield return new KeyValuePair<string, HashSet<int>>(currentTerm, currentSet);

                    enumerators = enumerators.Take(i).Where(e => e.MoveNext())
                        .Concat(enumerators.Skip(i))
                        .OrderBy(e => e.Current.Key)
                        .ToArray();
                }
            }
        }

        private static IEnumerable<Dictionary<string, HashSet<int>>> GetTermBlocks(IEnumerable<Document> documents, DocumentIdTracker idTracker)
        {
            //key: term, value: set of document paths
            var termDictionary = new Dictionary<string, HashSet<int>>();

            var tokenizer = new WordDocumentTokenizer();

            foreach (var doc in documents)
            {
                int id = idTracker.TrackDocumentId(doc);
                foreach (var term in tokenizer.Tokenize(doc.Text))
                {
                    if (!termDictionary.ContainsKey(term))
                    {
                        termDictionary[term] = new HashSet<int>();
                    }

                    termDictionary[term].Add(id);

                    if (termDictionary.Count >= TermsInBlock)
                    {
                        yield return termDictionary;
                        termDictionary = new Dictionary<string, HashSet<int>>();
                    }
                }
            }

            yield return termDictionary;
        }
    }
}
