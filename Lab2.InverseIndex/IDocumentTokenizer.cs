using System.Collections.Generic;

namespace Lab2.InverseIndex
{
    public interface IDocumentTokenizer<out T>
    {
        IEnumerable<T> Tokenize(string text);
    }
}