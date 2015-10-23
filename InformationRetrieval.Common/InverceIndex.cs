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

        public InverceIndex(IDocumentTokenizer<TTerm> tokenizer)
        {
            _tokenizer = tokenizer;
        }

        protected Dictionary<TDocument, HashSet<DocumentZone>> GetZoneDictionary(TTerm input)
        {
            Dictionary<TDocument, HashSet<DocumentZone>> zoneDictionary;
            return _dictionary.TryGetValue(input, out zoneDictionary)
                ? zoneDictionary
                : new Dictionary<TDocument, HashSet<DocumentZone>>();
        }

        public IEnumerable<Tuple<TDocument, decimal>> GetDocumentsWithScore(TTerm term)
        {
            var zoneDictionary = GetZoneDictionary(term);
            return
                zoneDictionary.Select(
                    pair => new Tuple<TDocument, decimal>(pair.Key, pair.Value.Sum(zone => zone.Weight)));
        }

        public HashSet<TDocument> GetDocumentSet(TTerm input, bool negate = false)
        {
            Dictionary<TDocument, HashSet<DocumentZone>> zoneDictionary = GetZoneDictionary(input);
            var result = new HashSet<TDocument>(zoneDictionary.Select(pair => pair.Key));
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
