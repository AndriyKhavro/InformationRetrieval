using System;
using System.Collections.Generic;

namespace Lab3.BiwordIndex
{
    public interface IPartOfSpeechTagger
    {
        IEnumerable<Tuple<string, PartOfSpeech>> ParseText(string text);
    }
}
