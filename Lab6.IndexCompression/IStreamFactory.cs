using System.IO;

namespace Lab6.IndexCompression
{
    public interface IStreamFactory
    {
        TextReader CreateStreamReader(string filepath);
        TextWriter CreateDictionaryStreamWriter(string filepath);
        Stream CreateDocumentIdsStream(string filepath, FileMode mode);
    }
}