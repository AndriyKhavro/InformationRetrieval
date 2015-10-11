using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace InformationRetrieval.Common
{
    public class DocumentProvider
    {
        public static IEnumerable<Document> GetDocuments(string directory)
        {
            var filePaths = Directory.GetFiles(directory).Where(file => file.EndsWith(".txt")).ToArray();
            return filePaths.Select(f => new Document
            {
                FilePath = f,
                Text = File.ReadAllText(f)
            });
        }
    }
}
