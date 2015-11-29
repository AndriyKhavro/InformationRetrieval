using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lab6.IndexCompression
{
    public class SimpleIndexSerializer : IIndexSerializer
    {
        private const char SplitSymbol = '\t';
        public void SerializeToFile(string filePath, Dictionary<string, HashSet<int>> termBlock)
        {
            File.WriteAllText(filePath, BlockToText(termBlock));
        }

        public void SerializeToFileByLine(string baseFilePath, IEnumerable<KeyValuePair<string, HashSet<int>>> pairs)
        {
            using (var streamWriter = new StreamWriter(baseFilePath))
            {
                foreach (var pair in pairs)
                {
                    streamWriter.WriteLine(GetBlockLine(pair.Key, pair.Value));
                }
            }
        }

        public IEnumerable<KeyValuePair<string, HashSet<int>>> DeserializeByLine(string filePath)
        {
            using (var file = new StreamReader(filePath))
            {
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    yield return LineToPair(line);
                }
            }
        }

        private static string BlockToText(Dictionary<string, HashSet<int>> block)
        {
            return string.Join("\n",
                block.OrderBy(o => o.Key)
                    .Select(
                        o =>
                            GetBlockLine(o.Key, o.Value)));
        }

        private static string GetBlockLine(string term, HashSet<int> set)
        {
            return $"{term}{SplitSymbol}{string.Join(SplitSymbol.ToString(), set)}";
        }

        private static KeyValuePair<string, HashSet<int>> LineToPair(string line)
        {
            var splittedLine = line.Split(SplitSymbol);
            return new KeyValuePair<string, HashSet<int>>(splittedLine[0],
                new HashSet<int>(splittedLine.Skip(1).Select(int.Parse)));
        }

    }
}
