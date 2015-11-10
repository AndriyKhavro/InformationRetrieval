using System.Collections.Generic;

namespace Lab6.IndexCompression
{
    public interface INumberEncoder
    {
        byte[] EncodeNumbers(IEnumerable<int> numbers);
        IEnumerable<int> DecodeNumbers(IEnumerable<byte> bytes);
    }
}