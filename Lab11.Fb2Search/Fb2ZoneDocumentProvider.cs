using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using InformationRetrieval.Common;
using Lab7.ZoneScoring;

namespace Lab11.Fb2Search
{
    public class Fb2ZoneDocumentProvider : IDocumentProvider
    {
        public IEnumerable<IDocument> GetDocuments(string directory)
        {
            var filePaths = Directory.GetFiles(directory).Where(file => file.EndsWith(".fb2")).ToArray();
            return filePaths.Select(path => new DocumentWithZones(ParseDocument(path)) { FilePath = path });
        }


        //assumption: body weight: 0.5; other tags weight: 0.5 / count
        private static IEnumerable<Tuple<DocumentZone, string>> ParseDocument(string filePath)
        {
            using (var reader = XmlReader.Create(filePath))
            {
                var xElement = XElement.Load(reader);

                var childElements = xElement.XPathSelectElements("child::*").ToArray();
                var description = childElements.FirstOrDefault(e => e.Name.LocalName == "description");
                var descriptionChildren = description?.XPathSelectElements("child::*").ToArray() ?? new XElement[0];

                var body = childElements.First(e => e.Name.LocalName == "body");

                return
                    descriptionChildren
                        .Select(e => new Tuple<DocumentZone, string>(
                            new DocumentZone(e.Name.LocalName,
                                0.5m/descriptionChildren.Length), e.Value))
                        .Concat(new[] {Tuple.Create(new DocumentZone("body", 0.5m), body.Value)});
            }
        }
    }
}
