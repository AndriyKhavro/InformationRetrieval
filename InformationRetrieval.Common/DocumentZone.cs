using System;

namespace InformationRetrieval.Common
{
    public class DocumentZone : IEquatable<DocumentZone>
    {
        public DocumentZone(string name, decimal weight)
        {
            Name = name;
            Weight = weight;
        }

        /// <summary>
        /// must be unique
        /// </summary>
        public string Name { get; }
        public decimal Weight { get; }

        public bool Equals(DocumentZone other)
        {
            return Name == other.Name;
        }

        public override bool Equals(object obj)
        {
            var zone = obj as DocumentZone;
            return zone != null && Equals(zone);
        }

        public override int GetHashCode()
        {
            return Name?.GetHashCode() ?? 0;
        }
    }
}
