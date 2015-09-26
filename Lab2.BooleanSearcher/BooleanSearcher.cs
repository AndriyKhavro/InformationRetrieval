using System;
using System.Collections.Generic;
using Lab2.InverseIndex;

namespace Lab2.BooleanSearcher
{
    public class BooleanSearcher
    {
        private readonly InverceIndex _inverceIndex;

        public BooleanSearcher(InverceIndex inverceIndex)
        {
            _inverceIndex = inverceIndex;
        }

        public HashSet<Document> ProcessQuery(string query)
        {
            var tokenizer = new DocumentTokenizer();
            var terms = tokenizer.GetWordsFromText(query);

            HashSet<Document> currentSet = null;
            Operator currentOperator = Operator.AND;
            bool negating = false;

            foreach (var term in terms)
            {
                switch (term)
                {
                    case "AND":
                    case "OR":
                        currentOperator = (Operator) Enum.Parse(typeof (Operator), term);
                        negating = false;
                        break;
                    case "NOT":
                        negating = true;
                        break;
                    default:
                        var newSet = _inverceIndex.GetDocumentSet(term, negating);
                        AddToSet(newSet, currentOperator, ref currentSet);
                        negating = false;
                        break;
                }
            }

            return currentSet ?? new HashSet<Document>();
        }

        private static void AddToSet(HashSet<Document> newSet, Operator oper, ref HashSet<Document> currentSet)
        {
            if (currentSet == null)
            {
                currentSet = new HashSet<Document>(newSet);
                return;
            }
            if (oper == Operator.AND)
            {
                currentSet.IntersectWith(newSet);
            }
            else
            {
                currentSet.UnionWith(newSet);
            }
        }
    }
}
