using System;
using System.Collections.Generic;
using System.Linq;
using InformationRetrieval.Common;

namespace Lab3.PositionalIndex
{
    public class PositionalIndex
    {
        private readonly Dictionary<string, Dictionary<Document, HashSet<int>>> _dictionary =
            new Dictionary<string, Dictionary<Document, HashSet<int>>>();

        private readonly IDocumentTokenizer<string> _tokenizer;

        public PositionalIndex(IDocumentTokenizer<string> tokenizer)
        {
            _tokenizer = tokenizer;
        }

        public IEnumerable<Document> FindDocuments(string input)
        {
            var words = _tokenizer.Tokenize(input).ToArray();
            if (words.Length == 0)
            {
                return Enumerable.Empty<Document>();
            }

            var dictionaries = words.Select(FindDocumentsByWord).ToArray();
            
            return
                dictionaries[0].Keys.Where(
                    doc =>
                        dictionaries.Where(
                            (dict, i) => dict[doc].Select(n => n - i).Intersect(dictionaries[0][doc]).Any()).Count() ==
                        dictionaries.Length);
        }

        private Dictionary<Document, HashSet<int>> FindDocumentsByWord(string word)
        {
            Dictionary<Document, HashSet<int>> result;
            return _dictionary.TryGetValue(word, out result) ? result : new Dictionary<Document, HashSet<int>>();
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
            var words = _tokenizer.Tokenize(document.Text).ToArray();
            for (int i = 0; i < words.Length; i++)
            {
                Add(new Tuple<string, int>(words[i], i), document);
            }
        }

        private void Add(Tuple<string, int> termWithPosition, Document document)
        {
            var term = termWithPosition.Item1;
            if (!_dictionary.ContainsKey(term))
            {
                _dictionary[term] = new Dictionary<Document, HashSet<int>>();
            }

            if (!_dictionary[term].ContainsKey(document))
            {
                _dictionary[term][document] = new HashSet<int>();
            }
            _dictionary[term][document].Add(termWithPosition.Item2);
        }
    }
}
