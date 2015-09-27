using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2.InverseIndex
{
    public static class HashSetHelper
    {
        public static void AddToSet<T>(HashSet<T> newSet, Operator oper, ref HashSet<T> currentSet)
        {
            if (currentSet == null)
            {
                currentSet = new HashSet<T>(newSet);
                return;
            }
            if (oper == Operator.AND)
            {
                currentSet.IntersectWith(newSet);
            }
            else
            {
                currentSet.UnionWith(newSet);
            }
        }
    }
}
