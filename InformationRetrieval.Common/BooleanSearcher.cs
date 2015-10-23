using System;
using System.Collections.Generic;

namespace InformationRetrieval.Common
{
    public class BooleanSearcher
    {
        private readonly InverceIndex<string, IDocument> _inverceIndex;

        public BooleanSearcher(InverceIndex<string, IDocument> inverceIndex)
        {
            _inverceIndex = inverceIndex;
        }

        public HashSet<IDocument> ProcessQuery(string query)
        {
            var tokenizer = new WordDocumentTokenizer();
            var terms = tokenizer.Tokenize(query);

            HashSet<IDocument> currentSet = null;
            Operator currentOperator = Operator.AND;
            bool negating = false;

            foreach (var term in terms)
            {
                switch (term)
                {
                    case "and":
                    case "or":
                        currentOperator = (Operator) Enum.Parse(typeof (Operator), term.ToUpper());
                        negating = false;
                        break;
                    case "not":
                        negating = true;
                        break;
                    default:
                        var newSet = _inverceIndex.GetDocumentSet(term, negating);
                        HashSetHelper.AddToSet(newSet, currentOperator, ref currentSet);
                        negating = false;
                        break;
                }
            }

            return currentSet ?? new HashSet<IDocument>();
        }
    }
}
