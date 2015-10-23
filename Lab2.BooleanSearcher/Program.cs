using System;
using System.Linq;
using InformationRetrieval.Common;

namespace Lab2.BooleanSearcher
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var inputDirectory = args[0];

            new BooleanSearchProgram(inputDirectory, new SimpleDocumentProvider()).Launch();
        }
    }
}
