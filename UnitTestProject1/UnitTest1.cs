using System;
using Lab3.BiwordIndex;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var tagger = new PartOfSpeechTagger();
            var text = "A Part-Of-Speech Tagger (POS Tagger) is a piece of software that reads text"
                       + "in some language and assigns parts of speech to each word (and other token),"
                       + " such as noun, verb, adjective, etc., although generally computational "
                       + "applications use more fine-grained POS tags like 'noun-plural'.";
            tagger.ParseText(text);
        }
    }
}
