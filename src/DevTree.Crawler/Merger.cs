using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTree.Crawler
{
    public class Merger
    {
        private string _savePath;
        private HashSet<string> _uniqueWords = new HashSet<string>();

        public Merger(string[] args)
        {
            _savePath = ParameterHelper.GetParameter(args, "-path", "Path");
        }

        public HashSet<string> Merge()
        {
            return GatherUniqueWords(_savePath);
        }

        private static HashSet<string> GatherUniqueWords(string directoryPath)
        {
            var files = Directory.GetFiles(directoryPath);
            var listOfWords = new HashSet<string>();

            foreach (var file in files)
            {
                Console.WriteLine($"Reading: {file}...");
                var words = File.ReadAllLines(file);

                int newWords = 0;
                foreach (var word in words)
                {
                    if (!listOfWords.Contains(word))
                    {
                        listOfWords.Add(word);
                        newWords++;
                    }
                }

                Console.WriteLine($"Words in the file: {words.Length:n0}");
                Console.WriteLine($"New Words: {newWords:n0}");
                Console.WriteLine($"Total Unique Words: {listOfWords.Count:n0}");
            }

            return listOfWords;
        }
    }
}
