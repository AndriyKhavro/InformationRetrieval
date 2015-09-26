using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Lab2.InverseIndex
{
    public class DocumentTokenizer
    {
        public IEnumerable<string> GetWordsFromText(string text)
        {
            return Regex.Split(text, "[^a-zA-Z']").Where(s => !string.IsNullOrEmpty(s));
        }
    }
}