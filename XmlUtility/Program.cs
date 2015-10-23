using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace XmlUtility
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = args[0];
            foreach (var file in Directory.GetFiles(path).Where(f => f.EndsWith(".txt")))
            {
                var text = File.ReadAllText(file);
                var xmlFilePath = $"{file}.xml";
                File.Delete(xmlFilePath);
                using (var writer = XmlWriter.Create(xmlFilePath))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("root");

                    writer.WriteStartElement("Author");

                    writer.WriteStartAttribute("weight");
                    writer.WriteValue(0.2);
                    writer.WriteEndAttribute();

                    writer.WriteValue("Andrii");
                    writer.WriteEndElement();

                    writer.WriteStartElement("Abstract");

                    writer.WriteStartAttribute("weight");
                    writer.WriteValue(0.3);
                    writer.WriteEndAttribute();

                    writer.WriteValue(text.Substring(0, 100));
                    writer.WriteEndElement();

                    writer.WriteStartElement("Text");

                    writer.WriteStartAttribute("weight");
                    writer.WriteValue(0.5);
                    writer.WriteEndAttribute();

                    writer.WriteValue(text);
                    writer.WriteEndElement();

                    writer.WriteEndElement();

                    writer.WriteEndDocument();
                }
            }
        }
    }
}
