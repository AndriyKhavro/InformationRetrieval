using System;
using System.Collections.Generic;

namespace Lab2.InverseIndex
{
    public class BooleanSearcher
    {
        private readonly InverceIndex<string> _inverceIndex;

        public BooleanSearcher(InverceIndex<string> inverceIndex)
        {
            _inverceIndex = inverceIndex;
        }

        public HashSet<Document> ProcessQuery(string query)
        {
            var tokenizer = new WordDocumentTokenizer();
            var terms = tokenizer.Tokenize(query);

            HashSet<Document> currentSet = null;
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

            return currentSet ?? new HashSet<Document>();
        }
    }
}
