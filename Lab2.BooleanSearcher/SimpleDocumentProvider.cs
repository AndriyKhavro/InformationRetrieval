using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InformationRetrieval.Common;

namespace Lab2.BooleanSearcher
{
    public class SimpleDocumentProvider : IDocumentProvider
    {
        public IEnumerable<IDocument> GetDocuments(string directory)
        {
            return DocumentProvider.GetDocuments(directory);
        }
    }
}
