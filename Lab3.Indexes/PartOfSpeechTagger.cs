using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Threading;
using edu.stanford.nlp.ling;
using edu.stanford.nlp.tagger.maxent;
using java.io;
using java.util;

namespace Lab3.BiwordIndex
{
    public class PartOfSpeechTagger : IPartOfSpeechTagger
    {
        private readonly string[] _nouns = {"NN", "NNS", "NNP", "NNPS"};

        public IEnumerable<Tuple<string, PartOfSpeech>> ParseText(string text)
        {
            var jarRoot = (string) (new AppSettingsReader().GetValue("POSTaggerRoot", typeof (string)));
            var modelsDirectory = jarRoot + @"\models";

            // Some black magic just to load POS Tagger
            CultureInfo ci = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
            
            var tagger = new MaxentTagger(modelsDirectory + @"\wsj-0-18-bidirectional-nodistsim.tagger");

            var sentences = MaxentTagger.tokenizeText(new StringReader(text)).toArray();
            
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (ArrayList sentence in sentences)
            {
                var taggedSentence = tagger.tagSentence(sentence).toArray();
                foreach (TaggedWord word in taggedSentence)
                {
                    yield return
                        new Tuple<string, PartOfSpeech>(word.value(),
                            _nouns.Contains(word.tag()) ? PartOfSpeech.N : PartOfSpeech.X);
                }
            }
        }
    }
}
