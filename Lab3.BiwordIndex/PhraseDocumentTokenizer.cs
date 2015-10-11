using System.Collections.Generic;
using System.Linq;
using InformationRetrieval.Common;

namespace Lab3.BiwordIndex
{
    public class PhraseDocumentTokenizer : IDocumentTokenizer<Phrase>
    {
        private readonly IPartOfSpeechTagger _partOfSpeechTagger;

        public PhraseDocumentTokenizer(IPartOfSpeechTagger partOfSpeechTagger)
        {
            _partOfSpeechTagger = partOfSpeechTagger;
        }

        public IEnumerable<Phrase> Tokenize(string text)
        {
            string first = null;
            foreach (
                var word in
                    _partOfSpeechTagger.ParseText(text)
                        .Where(tuple => tuple.Item2 == PartOfSpeech.N)
                        .Select(s => s.Item1.ToLower()))
            {
                if (first != null)
                {
                    yield return new Phrase(first, word);
                }

                first = word;
            }
        }
    }
}
