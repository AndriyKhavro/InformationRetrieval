using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InformationRetrieval.Common
{
    public interface IDocument
    {
        string FilePath { get; set; }
        IEnumerable<Tuple<DocumentZone, string>> Zones { get; }
    }}
