using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTree.Crawler
{
    public static class IOHelper
    {
        public static void SaveFile(string path, string contents)
        {
            EnsureDirectoryExists(path);

            File.WriteAllText(path, contents);
        }

        public static void SaveFile(string path, string[] lines)
        {
            EnsureDirectoryExists(path);

            File.WriteAllLines(path, lines);
        }

        public static void EnsureDirectoryExists(string path)
        {
            var directory = Path.GetDirectoryName(path);

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
        }
    }
}
