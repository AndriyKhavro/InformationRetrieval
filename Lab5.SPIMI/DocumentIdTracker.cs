using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InformationRetrieval.Common;

namespace Lab5.SPIMI
{
    public class DocumentIdTracker
    {
        private int _index = 1;
        private readonly string _documentIdFile;
        private const char SplitSymbol = '\t';

        public DocumentIdTracker(string outputDir)
        {
            _documentIdFile = Path.Combine(outputDir, "documentIds.txt");
        }

        public int TrackDocumentId(Document document)
        {
            File.AppendAllLines(_documentIdFile, new[] {$"{document.FilePath}{SplitSymbol}{_index}"});
            return _index++;
        }
    }
}
