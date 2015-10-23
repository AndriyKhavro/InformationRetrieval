using System.Collections.Generic;

namespace InformationRetrieval.Common
{
    public interface IDocumentTokenizer<out T>
    {
        IEnumerable<T> Tokenize(string text);
    }
}