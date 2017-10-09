using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTree.Crawler
{
    public class KurdishStringComparer : IComparer<string>
    {
        public int Compare(string first, string second)
        {
            //if (first == null)
            //{
            //    if (second == null) return 0;
            //    return -1;
            //}
            //if (second == null) return 1;

            //for (int i = 0; i < first.Length && i < second.Length; i++)
            //{
            //    var difference = Kurdish.Weight(first[i]) - Kurdish.Weight(second[i]);
            //    if (difference != 0) return difference;
            //}

            //if (first.Length == second.Length) return 0;
            //return first.Length < second.Length ? -1 : 1;

            return 0;
        }
    }
}
