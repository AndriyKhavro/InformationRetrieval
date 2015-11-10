using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lab6.IndexCompression
{
    public class CompressingIndexSerializer : IIndexSerializer
    {
        //we store dictionary and postings in two separate files
        private const string DictionaryFileSuffix = "dictionary.txt";
        private const string PostingsFileSuffix = "postings";
        private readonly INumberEncoder _encoder;
        private readonly IStreamFactory _factory;

        public CompressingIndexSerializer(IStreamFactory factory, INumberEncoder encoder)
        {
            _factory = factory;
            _encoder = encoder;
        }


        public void SerializeToFile(string filePath, Dictionary<string, HashSet<int>> termBlock)
        {
            SerializeToFileByLine(filePath, termBlock.OrderBy(pair => pair.Key));
        }

        public void SerializeToFileByLine(string filePath, IEnumerable<KeyValuePair<string, HashSet<int>>> pairs)
        {
            using (var dictionaryStreamWriter = _factory.CreateStreamWriter(GetDictionaryFilePath(filePath)))
            using (var postingsStream = _factory.CreateFileStream(GetPostingsFilePath(filePath), FileMode.Create))
            {
                foreach (var pair in pairs)
                {
                    dictionaryStreamWriter.WriteLine(pair.Key);
                    var bytes = _encoder.EncodeNumbers(GetNumbersForEncoding(pair.Value));
                    postingsStream.Write(bytes, 0, bytes.Length);
                }
            }
        }

        public IEnumerable<KeyValuePair<string, HashSet<int>>> DeserializeByLine(string filePath)
        {
            using (var dictionaryStreamReader = _factory.CreateStreamReader(GetDictionaryFilePath(filePath)))
            using (var postingsStream = _factory.CreateFileStream(GetPostingsFilePath(filePath), FileMode.Open))
            {
                var postingSet = new HashSet<int>();
                int prev = 0;
                //We read postingsStream until we find encoded zero. Than we switch to the next term. Keeping in mind that we store difference between numbers
                foreach (var value in _encoder.DecodeNumbers(ReadByByte(postingsStream)))
                {
                    if (value == 0)
                    {
                        yield return
                            new KeyValuePair<string, HashSet<int>>(dictionaryStreamReader.ReadLine(), postingSet);
                        postingSet = new HashSet<int>();
                        prev = 0;
                    }
                    else
                    {
                        prev += value;
                        postingSet.Add(prev);
                    }
                }
            }
        }

        private static IEnumerable<byte> ReadByByte(Stream stream)
        {
            int x;
            while ((x = stream.ReadByte()) != -1)
            {
                yield return (byte) x;
            }
        }

        private static string GetDictionaryFilePath(string baseFilePath)
        {
            return $"{baseFilePath}.{DictionaryFileSuffix}";
        }

        private static string GetPostingsFilePath(string baseFilePath)
        {
            return $"{baseFilePath}.{PostingsFileSuffix}";
        }

        //returns difference between numbers to reduce number of bytes. separates sets by zeros
        private static IEnumerable<int> GetNumbersForEncoding(HashSet<int> postingSet)
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
