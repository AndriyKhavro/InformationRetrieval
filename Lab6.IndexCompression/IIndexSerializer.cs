using System.Collections.Generic;

namespace Lab6.IndexCompression
{
    public interface IIndexSerializer
    {
        //HashSet<int> - documentIds
        void SerializeToFile(string filePath, Dictionary<string, HashSet<int>> termBlock);
        void SerializeToFileByLine(string baseFilePath, IEnumerable<KeyValuePair<string, HashSet<int>>> pairs);
        IEnumerable<KeyValuePair<string, HashSet<int>>> DeserializeByLine(string filePath);
    }
}