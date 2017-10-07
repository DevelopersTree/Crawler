using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTree.Crawler
{
    public class Normalizer
    {
        private readonly string _path;
        public Normalizer(string path)
        {
            _path = path;
        }

        public HashSet<char> Normalize()
        {
            var list = new HashSet<char>();

            var file = File.ReadAllText(_path);

            foreach(var c in file)
            {
                list.Add(c);
            }

            return list;
        }
    }
}
