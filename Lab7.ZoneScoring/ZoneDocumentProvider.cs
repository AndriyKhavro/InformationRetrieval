using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using InformationRetrieval.Common;

namespace Lab7.ZoneScoring
{
    public class ZoneDocumentProvider : IDocumentProvider
    {
        public IEnumerable<IDocument> GetDocuments(string directory)
        {
            var filePaths = Directory.GetFiles(directory).Where(file => file.EndsWith(".xml")).ToArray();
            return filePaths.Select(path => new DocumentWithZones(ParseDocument(path)) {FilePath = path});
        }

        private static IEnumerable<Tuple<DocumentZone, string>> ParseDocument(string filePath)
        {
            using (var reader = XmlReader.Create(filePath))
            {
                var xElement = XElement.Load(reader);

                return
                    xElement.XPathSelectElements("child::*")
                        .Select(
                            e =>
                                new Tuple<DocumentZone, string>(
                                    new DocumentZone(e.Name.LocalName,
                                        Decimal.Parse(e.Attribute(XName.Get("weight")).Value, NumberFormatInfo.InvariantInfo)), e.Value)).ToArray();
            }
        }
    }
}
