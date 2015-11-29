using System.Collections.Generic;
using System.Linq;

namespace Lab6.IndexCompression
{
    public class NumberLengthReducer : INumberLengthReducer
    {
        //returns difference between numbers to reduce number of bytes. separates sets by zeros
        public IEnumerable<int> GetNumbersForEncoding(IEnumerable<int> postingSet)
        {
            int prev = 0;
            foreach (var i in postingSet.OrderBy(i => i))
            {
                yield return i - prev;
                prev = i;
            }

            yield return 0;
        }
    }
}
