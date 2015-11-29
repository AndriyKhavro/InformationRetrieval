using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InformationRetrieval.Common;

namespace Lab7.ZoneScoring
{
    public class DocumentWithZones : IDocument
    {
        private readonly Tuple<DocumentZone, string>[] _tuples;

        public DocumentWithZones(IEnumerable<Tuple<DocumentZone, string>> tuples)
        {
            _tuples = tuples.ToArray();
        }

        public string FilePath { get; set; }

        /// <summary>
        /// Collection of tuples. Item1 - DocumentZone (name of zone and weight), Item2 - text inside the zone
        /// </summary>
        public IEnumerable<Tuple<DocumentZone, string>> Zones => _tuples;
    }
}
