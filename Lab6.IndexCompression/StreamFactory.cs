using System.IO;

namespace Lab6.IndexCompression
{
    public class StreamFactory : IStreamFactory
    {
        public TextReader CreateStreamReader(string filepath)
        {
            return new StreamReader(filepath);
        }

        public TextWriter CreateStreamWriter(string filepath)
        {
            return new StreamWriter(filepath);
        }

        public Stream CreateFileStream(string filepath, FileMode mode)
        {
            return new FileStream(filepath, mode);
        }
    }
}
