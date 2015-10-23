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
            var path = @"D:\univ\files";
            foreach (var file in Directory.GetFiles(path))
            {
                var text = File.ReadAllText(file);
                XmlWriter writer = XmlWriter.Create($"{file}.xml");
                writer.WriteStartDocument();
                writer.WriteStartElement("root");

                writer.WriteStartElement("Author");
                writer.WriteValue("Andrii");
                writer.WriteEndElement();

                writer.WriteStartElement("Abstract");
                writer.WriteValue(text.Substring(0, 100));
                writer.WriteEndElement();

                writer.WriteStartElement("Text");
                writer.WriteValue(text);
                writer.WriteEndElement();

                writer.WriteEndElement();

                writer.WriteEndDocument();
                
                writer.Flush();
            }
        }
    }
}
