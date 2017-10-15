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
        private static char[] _delimiters = {' ','،',',',';','.','؛',':','.','/','\\' };


        public static void wordlist(string[] args)
        {
            string inputdir = ParameterHelper.GetParameter(args, "-inputdir", " Input Directory ");
            string filename = ParameterHelper.GetParameter(args, "-outfile", " Output File Name ");

            DirectoryInfo dinfo = new DirectoryInfo(inputdir);
            FileInfo[] TextFiles = dinfo.GetFiles("*.txt");
            //ConsoleUtil.DrawTextProgressBar(0, TextFiles.Length);
            string[] tmpStrArray = null;
            string wordlist = "";
            
            int i = 0;
            Console.WriteLine("---------Creating wordlist---------");
            var characters = Unicode.SoraniNumbers.ToArray();
            foreach (FileInfo file in TextFiles)
            {
                tmpStrArray = File.ReadAllText(file.FullName).Split();
                foreach(string element in tmpStrArray)
                {
                    string[] test = { "1","2"};
                    if (!wordlist.Contains(element) && element.IndexOfAny(characters)==-1)
                    {
                        wordlist = wordlist + "\n" + element;
                    }
                }
                ConsoleUtil.DrawTextProgressBar(i, TextFiles.Length);
                i++;
                tmpStrArray = null;
            }
            Console.WriteLine("\n---------wordlist created---------");
            Console.WriteLine("---------Saving Results---------");
            IOHelper.SaveFile(inputdir+"/"+filename, wordlist);
            Console.WriteLine("---------Done---------");
        }
    }
}
