using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevTree.BeKurdi;

namespace DevTree.Crawler
{
    class WordList
    {
        private static char[] _delimiters = { ' ', '،', ',', ';', '.', '؛', ':', '.', '/', '\\', '\n','_',':','-' };
        private static Dictionary<string, int> _wordList;

        public static void wordlist(string[] args)
        {
            string inputdir = ParameterHelper.GetParameter(args, "-inputdir", " Input Directory ");
            string filename = ParameterHelper.GetParameter(args, "-outfile", " Output File Name ");
            _wordList = new Dictionary<string, int>();

            DirectoryInfo dinfo = new DirectoryInfo(inputdir);
            FileInfo[] TextFiles = dinfo.GetFiles("*.txt", SearchOption.AllDirectories);
            string[] tmpStrArray = null;

            int i = 0;
            Console.WriteLine("---------Creating wordlist---------");
            var characters = Unicode.SoraniNumbers.ToArray();
            foreach (FileInfo file in TextFiles)
            {
                tmpStrArray = File.ReadAllText(file.FullName).Split(_delimiters);
                foreach (string element in tmpStrArray)
                {
                    if (element.IndexOfAny(characters) != -1)
                        continue;

                    if (_wordList.ContainsKey(element))
                    {
                        _wordList[element]++;
                    }
                    else
                    {
                        _wordList.Add(element, 1);
                    }
                }

                ConsoleUtil.DrawTextProgressBar(i, TextFiles.Length);
                i++;
            }

            Console.WriteLine(Environment.NewLine + $"---------wordlist created: {_wordList.Count:n0}---------");
            Console.WriteLine("---------Saving Results---------");

            var builder = new StringBuilder(_wordList.Count * 5);

            foreach (KeyValuePair<string, int> pair in _wordList)
            {
               builder.Append(pair.Key + "," + pair.Value + Environment.NewLine);
            }
            
            IOHelper.SaveFile(Path.Combine(inputdir, filename), builder.ToString());
            Console.WriteLine("---------Done---------");
        }
    }
}
