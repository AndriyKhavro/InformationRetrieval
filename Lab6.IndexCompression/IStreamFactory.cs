using System.IO;

namespace Lab6.IndexCompression
{
    public interface IStreamFactory
    {
        TextReader CreateStreamReader(string filepath);
        TextWriter CreateStreamWriter(string filepath);
        Stream CreateFileStream(string filepath, FileMode mode);
    }
}