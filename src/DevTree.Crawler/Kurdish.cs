using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTree.Crawler
{
    public class Kurdish
    {
        // from: http://unicode.ekrg.org/ku_unicodes.html
        public static char[] CharacterSet => new char[]
        {
            '\u0626', '\u0627', '\u0628', '\u067E', '\u062A', '\u062C', '\u0686', '\u062D', '\u062E', '\u062F', '\u0631',
            '\u0695', '\u0632', '\u0698', '\u0633', '\u0634', '\u0639', '\u063A', '\u0641', '\u06A4', '\u0642', '\u06A9',
            '\u06AF', '\u0644', '\u06B5', '\u0645', '\u0646', '\u0648', '\u06C6', '\u0647', '\u06BE' /* Wikipedia haa */,
            '\u06D5', '\u06CC', '\u06CE', '\u0020' /* Space */ 
        };

        private static Dictionary<char, int> _weights;

        public static int Weight(char character)
        {
            if (_weights == null)
                _weights = CharacterSet.ToDictionary(c => c, c => Array.IndexOf(CharacterSet, c));

            if (_weights.ContainsKey(character))
                return _weights[character] + 1;
            else
            {
                return 0;
            }
        }
    }
}
