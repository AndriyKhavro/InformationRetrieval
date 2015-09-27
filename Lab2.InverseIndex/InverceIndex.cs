using System.Collections.Generic;
using System.Linq;

namespace Lab2.InverseIndex
{
    /// <summary>
    /// Includes functionality of incidence matrix
    /// </summary>
    public class InverceIndex<T>
    {
        private readonly Dictionary<T, HashSet<Document>> _dictionary = new Dictionary<T, HashSet<Document>>();

        private readonly IDocumentTokenizer<T> _tokenizer;
        private readonly HashSet<Document> _allDocuments = new HashSet<Document>();

        public InverceIndex(IDocumentTokenizer<T> tokenizer)
        {
            _tokenizer = tokenizer;
        }

        public HashSet<Document> GetDocumentSet(T input, bool negate = false)
        {
            HashSet<Document> result;
            result = _dictionary.TryGetValue(input, out result) ? result : new HashSet<Document>();
            return negate ? new HashSet<Document>(_allDocuments.Except(result)) : result;
        }

        public void AddDocuments(IEnumerable<Document> documents)
        {
            foreach (var document in documents)
            {
                Add(document);
            }
        }
        
        private void Add(Document document)
        {
            var words = _tokenizer.Tokenize(document.Text);
            foreach (var word in words)
            {
                Add(word, document);
            }

            _allDocuments.Add(document);
        }

        private void Add(T term, Document document)
        {
            if (!_dictionary.ContainsKey(term))
            {
                _dictionary[term] = new HashSet<Document>();
            }

            _dictionary[term].Add(document);
        }
    }
}
