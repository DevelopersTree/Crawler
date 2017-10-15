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
        private static char[] _delimiters = {' ','،',',',';','.','؛',':','.','/','\\','\n'};
        private static Dictionary<string,int> _duplicates;

        public static void wordlist(string[] args)
        {
            string inputdir = ParameterHelper.GetParameter(args, "-inputdir", " Input Directory ");
            string filename = ParameterHelper.GetParameter(args, "-outfile", " Output File Name ");
            _duplicates = new Dictionary<string, int>();

            DirectoryInfo dinfo = new DirectoryInfo(inputdir);
            FileInfo[] TextFiles = dinfo.GetFiles("*.txt", SearchOption.AllDirectories);
            string[] tmpStrArray = null;
            string wordlist = "";
            
            int i = 0;
            Console.WriteLine("---------Creating wordlist---------");
            var characters = Unicode.SoraniNumbers.ToArray();
            Word tmpWord = new Word();
            foreach (FileInfo file in TextFiles)
            {
                tmpStrArray = File.ReadAllText(file.FullName).Split(_delimiters);
                foreach(string element in tmpStrArray)
                {
                    
                    if (!wordlist.Contains(element) && element.IndexOfAny(characters)==-1)
                    {
                        wordlist = wordlist + Environment.NewLine + element;
                    }
                    else
                    {
                        if (_duplicates.ContainsKey(element))
                        {
                            _duplicates[element]++;
                        }
                        else
                        {
                            _duplicates.Add(element, 1);
                        }
                    }
                }
                ConsoleUtil.DrawTextProgressBar(i, TextFiles.Length);
                i++;
                tmpStrArray = null;
            }
            string statstics = "";
            foreach (KeyValuePair<string, int> pair in _duplicates)
            {
                statstics = statstics  + pair.Key + "," + pair.Value + Environment.NewLine;
            }
            IOHelper.SaveFile(Path.Combine(inputdir, "/STATSTICS.txt"), statstics);
            Console.WriteLine(Environment.NewLine + "---------wordlist created---------");
            Console.WriteLine("---------Saving Results---------");
            IOHelper.SaveFile(Path.Combine(inputdir,filename), wordlist);
            Console.WriteLine("---------Done---------");
        }
    }
}
