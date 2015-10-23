using System;
using System.Collections.Generic;

namespace InformationRetrieval.Common
{
    public class Document : IDocument
    {
        public string Text { get; set; }
        public string FilePath { get; set; }

        private readonly DocumentZone _zone = new DocumentZone("Text", 1);

        public IEnumerable<Tuple<DocumentZone, string>> Zones
        {
            get { yield return new Tuple<DocumentZone, string>(_zone, Text); }
        }
    }
}
