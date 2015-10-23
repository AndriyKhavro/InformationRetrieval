using System;
using System.Collections.Generic;
using System.Linq;

namespace InformationRetrieval.Common
{
    /// <summary>
    /// Includes functionality of incidence matrix
    /// </summary>
    public class InverceIndex<TTerm, TDocument> where TDocument: IDocument
    {
        private readonly Dictionary<TTerm, Dictionary<TDocument, HashSet<DocumentZone>>> _dictionary =
            new Dictionary<TTerm, Dictionary<TDocument, HashSet<DocumentZone>>>();

        private readonly IDocumentTokenizer<TTerm> _tokenizer;
        private readonly HashSet<TDocument> _allDocuments = new HashSet<TDocument>();

        private const decimal EDGE_VALUE = 0.5m;

        public InverceIndex(IDocumentTokenizer<TTerm> tokenizer)
        {
            _tokenizer = tokenizer;
        }

        public HashSet<TDocument> GetDocumentSet(TTerm input, bool negate = false)
        {
            Dictionary<TDocument, HashSet<DocumentZone>> zoneDictionary;
            var result = _dictionary.TryGetValue(input, out zoneDictionary)
                ? new HashSet<TDocument>(
                    zoneDictionary.Where(pair => pair.Value.Sum(zone => zone.Weight) >= EDGE_VALUE)
                        .Select(pair => pair.Key))
                : new HashSet<TDocument>();
            return negate ? new HashSet<TDocument>(_allDocuments.Except(result)) : result;
        }

        public void AddDocuments(IEnumerable<TDocument> documents)
        {
            foreach (var document in documents)
            {
                Add(document);
            }
        }
        
        private void Add(TDocument document)
        {
            //Item1 - zone; Item2 - text;
            foreach (var tuple in document.Zones)
            {
                var words = _tokenizer.Tokenize(tuple.Item2);
                foreach (var word in words)
                {
                    Add(word, document, tuple.Item1);
                }

                _allDocuments.Add(document);
            }
        }

        private void Add(TTerm term, TDocument document, DocumentZone zone)
        {
            if (!_dictionary.ContainsKey(term))
            {
                _dictionary[term] = new Dictionary<TDocument, HashSet<DocumentZone>>();
            }

            if (!_dictionary[term].ContainsKey(document))
            {
                _dictionary[term][document] = new HashSet<DocumentZone>();
            }

            _dictionary[term][document].Add(zone);
        }
    }
}
