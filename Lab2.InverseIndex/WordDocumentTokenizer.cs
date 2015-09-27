using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Lab2.InverseIndex
{
    public class WordDocumentTokenizer : IDocumentTokenizer<string>
    {
        public IEnumerable<string> Tokenize(string text)
        {
            return
                Regex.Split(text, "[^a-zA-Z']")
                    .Where(s => !string.IsNullOrEmpty(s))
                    .Select(s => s.ToLower())
                    .Where(s => s != "and" && s != "or" && s != "not");
        }
    }
}