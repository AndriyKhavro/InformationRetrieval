using System.Collections.Generic;
using InformationRetrieval.Common;

namespace Lab3.BiwordIndex
{
    public class PhraseQueryExecutor
    {
        private readonly InverceIndex<Phrase> _inverceIndex;

        public PhraseQueryExecutor(InverceIndex<Phrase> inverceIndex)
        {
            _inverceIndex = inverceIndex;
        }

        public HashSet<Document> ProcessQuery(string query)
        {
            var tokenizer = new PhraseDocumentTokenizer(new PartOfSpeechTagger());
            var phrases = tokenizer.Tokenize(query);

            HashSet<Document> currentSet = null;

            foreach (var phrase in phrases)
            {
                var newSet = _inverceIndex.GetDocumentSet(phrase);
                HashSetHelper.AddToSet(newSet, Operator.AND, ref currentSet);
            }

            return currentSet ?? new HashSet<Document>();
        }
    }
}
