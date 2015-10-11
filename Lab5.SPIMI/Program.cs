using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InformationRetrieval.Common;

namespace Lab5.SPIMI
{
    internal class Program
    {
        private const int TermsInBlock = 10000;
        private const char SplitSymbol = '\t';
        private const string InputDir = @"C:\university\6\ir\files";
        private const string OutputDir = @"C:\university\6\ir\OutputDir";

        private static void Main(string[] args)
        {
            var docs = DocumentProvider.GetDocuments(InputDir);

            var termBlocks = GetTermBlocks(docs);

            int i = 1;

            //we write all terms dictionaries in files
            foreach (var outputText in termBlocks.Select(BlockToText))
            {
                File.WriteAllText(Path.Combine(OutputDir, $"{i++}.txt"), outputText);
            }

            //and then we just need to merge it simulteneously reading from all created files
            var fileNames = Enumerable.Range(1, i-1).Select(num => Path.Combine(OutputDir, $"{num}.txt")).ToArray();

            var mergedLines = MergeBlocks(fileNames.Select(file => ReadFileByLine(file).Select(line =>
            {
                var splittedLine = line.Split(SplitSymbol);
                return new KeyValuePair<string, HashSet<string>>(splittedLine[0],
                    new HashSet<string>(splittedLine.Skip(1)));
            })));

            WriteToFileByLine(mergedLines, Path.Combine(OutputDir, "output.txt"));
            foreach (var fileName in fileNames)
            {
                File.Delete(fileName);
            }
            Console.WriteLine("End");
            Console.ReadKey();
        }

        private static IEnumerable<string> MergeBlocks(IEnumerable<IEnumerable<KeyValuePair<string, HashSet<string>>>> blocks)
        {
            var enumerators =
                blocks.Select(b => b.GetEnumerator()).Where(e => e.MoveNext()).OrderBy(e => e.Current.Key).ToArray();
            while (enumerators.Any())
            {
                var firstEnumerator = enumerators.First();
                string currentTerm = firstEnumerator.Current.Key;
                var currentSet = new HashSet<string>(firstEnumerator.Current.Value);
                if (enumerators.Length == 1)
                {
                    yield return GetBlockLine(currentTerm, currentSet);
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

                    yield return GetBlockLine(currentTerm, currentSet);

                    enumerators = enumerators.Take(i).Where(e => e.MoveNext())
                        .Concat(enumerators.Skip(i))
                        .OrderBy(e => e.Current.Key)
                        .ToArray();
                }
            }
        }

        private static void WriteToFileByLine(IEnumerable<string> lines, string path)
        {
            using (var streamWriter = new StreamWriter(path))
            {
                foreach (var line in lines)
                {
                    streamWriter.WriteLine(line);
                }
            }
        }

        private static IEnumerable<string> ReadFileByLine(string fileName)
        {
            using (var file = new StreamReader(fileName))
            {
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        } 

        private static string BlockToText(Dictionary<string, HashSet<string>> block)
        {
            return string.Join("\n",
                block.OrderBy(o => o.Key)
                    .Select(
                        o =>
                            GetBlockLine(o.Key, o.Value)));
        }

        private static string GetBlockLine(string term, HashSet<string> set)
        {
            return $"{term}{SplitSymbol}{string.Join(SplitSymbol.ToString(), set)}";
        }

        private static IEnumerable<Dictionary<string, HashSet<string>>> GetTermBlocks(IEnumerable<Document> documents)
        {
            //key: term, value: set of document paths
            var termDictionary = new Dictionary<string, HashSet<string>>();

            var tokenizer = new WordDocumentTokenizer();

            foreach (var doc in documents)
            {
                foreach (var term in tokenizer.Tokenize(doc.Text))
                {
                    if (!termDictionary.ContainsKey(term))
                    {
                        termDictionary[term] = new HashSet<string>();
                    }

                    termDictionary[term].Add(doc.FilePath);

                    if (termDictionary.Count >= TermsInBlock)
                    {
                        yield return termDictionary;
                        termDictionary = new Dictionary<string, HashSet<string>>();
                    }
                }
            }

            yield return termDictionary;
        }
    }
}
