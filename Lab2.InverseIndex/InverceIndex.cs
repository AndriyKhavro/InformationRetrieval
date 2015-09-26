using System.Collections.Generic;
using System.Linq;

namespace Lab2.InverseIndex
{
    /// <summary>
    /// Includes functionality of incidence matrix
    /// </summary>
    public class InverceIndex
    {
        private readonly Dictionary<string, HashSet<Document>> _dictionary = new Dictionary<string, HashSet<Document>>();

        private readonly DocumentTokenizer _tokenizer;
        private readonly HashSet<Document> _allDocuments = new HashSet<Document>(); 

        public InverceIndex(DocumentTokenizer tokenizer)
        {
            _tokenizer = tokenizer;
        }

        public HashSet<Document> GetDocumentSet(string input, bool negate = false)
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

        public void Add(Document document)
        {
            var words = _tokenizer.GetWordsFromText(document.Text).Select(w => w.ToLower());
            foreach (var word in words)
            {
                Add(word, document);
            }

            _allDocuments.Add(document);
        }

        private void Add(string term, Document document)
        {
            if (!_dictionary.ContainsKey(term))
            {
                _dictionary[term] = new HashSet<Document>();
            }

            _dictionary[term].Add(document);
        }
    }
}
