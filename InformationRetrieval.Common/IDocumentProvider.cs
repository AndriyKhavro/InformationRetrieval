using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InformationRetrieval.Common
{
    public interface IDocumentProvider
    {
        IEnumerable<IDocument> GetDocuments(string directory);
    }
}
