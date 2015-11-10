using System.Collections.Generic;
using System.Linq;

namespace Lab6.IndexCompression
{
    public class VariableByteNumberEncoder : INumberEncoder
    {
        public byte[] EncodeNumbers(IEnumerable<int> numbers)
        {
            return numbers.SelectMany(EncodeNumber).ToArray();
        }

        public IEnumerable<int> DecodeNumbers(IEnumerable<byte> bytes)
        {
            int n = 0;
            foreach (var @byte in bytes)
            {
                if (@byte < 128)
                {
                    n = 128*n + @byte;
                }
                else
                {
                    n = 128*n + @byte - 128;
                    yield return n;
                    n = 0;
                }
            }
        }

        private IEnumerable<byte> EncodeNumber(int n)
        {
            var stack = new Stack<byte>();
            stack.Push((byte) (n%128 + 128));
            while (n >= 128)
            {
                n /= 128;
                stack.Push((byte) (n%128));
            }

            return stack;
        }
    }
}
