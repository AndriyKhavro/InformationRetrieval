using System.Collections.Generic;

namespace Lab6.IndexCompression
{
    public interface IIndexSerializer
    {
        void SerializeToFile(string filePath, Dictionary<string, HashSet<string>> termBlock);
        void SerializeToFileByLine(string filePath, IEnumerable<KeyValuePair<string, HashSet<string>>> pairs);
        IEnumerable<KeyValuePair<string, HashSet<string>>> DeserializeByLine(string filePath);
    }
}