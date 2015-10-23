using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab6.IndexCompression
{
    public static class IndexSerializerFactory
    {
        public static IIndexSerializer Create(Compression compression)
        {
            switch (compression)
            {
                case Compression.No:
                    return new SimpleIndexSerializer();
                case Compression.Yes:
                    return new CompressingIndexSerializer();
                default:
                    throw new ArgumentOutOfRangeException(nameof(compression), compression, "Unknown compression type");
            }
        }
    }
}
