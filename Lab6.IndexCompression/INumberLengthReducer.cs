using System.Collections.Generic;

namespace Lab6.IndexCompression
{
    public interface INumberLengthReducer
    {
        IEnumerable<int> GetNumbersForEncoding(IEnumerable<int> postingSet);
    }
}