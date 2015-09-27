using System;

namespace Lab3.BiwordIndex
{
    public class Phrase : IEquatable<Phrase>
    {
        public Phrase(string first, string second)
        {
            First = first;
            Second = second;
        }

        public string First { get; }
        public string Second { get; }

        public bool Equals(Phrase other)
        {
            if (other == null)
                return false;

            return (First == other.First && Second == other.Second);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Phrase);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((First?.GetHashCode() ?? 0)*397) ^ (Second?.GetHashCode() ?? 0);
            }
        }
    }
}
