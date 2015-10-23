using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab2.BooleanSearcher;

namespace Lab7.ZoneScoring
{
    class Program
    {
        static void Main(string[] args)
        {
            //var directory = args[0];
            var directory = @"D:\univ\files";
            new BooleanSearchProgram(directory, new ZoneDocumentProvider()).Launch();
        }
    }
}
