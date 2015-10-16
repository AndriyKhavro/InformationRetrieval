using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lab6.IndexCompression
{
    public class SimpleIndexSerializer : IIndexSerializer
    {
        private const char SplitSymbol = '\t';
        public void SerializeToFile(string filePath, Dictionary<string, HashSet<string>> termBlock)
        {
            File.WriteAllText(filePath, BlockToText(termBlock));
        }

        public void SerializeToFileByLine(string filePath, IEnumerable<KeyValuePair<string, HashSet<string>>> pairs)
        {
            using (var streamWriter = new StreamWriter(filePath))
            {
                foreach (var pair in pairs)
                {
                    streamWriter.WriteLine(GetBlockLine(pair.Key, pair.Value));
                }
            }
        }

        public IEnumerable<KeyValuePair<string, HashSet<string>>> DeserializeByLine(string filePath)
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

        private static KeyValuePair<string, HashSet<string>> LineToPair(string line)
        {
            var splittedLine = line.Split(SplitSymbol);
            return new KeyValuePair<string, HashSet<string>>(splittedLine[0],
                new HashSet<string>(splittedLine.Skip(1)));
        }

    }
}
