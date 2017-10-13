using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevTree.BeKurdi;
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
        public static void ConvertToStandardText(string[] args)
        {
            string _savedFilesDirectory = ParameterHelper.GetParameter(args, "-inputdir", " Input Directory ");
            string _outputDirectory = ParameterHelper.GetParameter(args, "-outdir", " Output Directory ");

            DirectoryInfo dinfo = new DirectoryInfo(_savedFilesDirectory);
            FileInfo[] TextFiles = dinfo.GetFiles("*.txt");
            ConsoleUtil.DrawTextProgressBar(0, TextFiles.Length);
            string tmpStr = null;
            int i = 0;
            foreach (FileInfo file in TextFiles)
            {
                tmpStr = (File.ReadAllText(file.FullName)).ToStandardSorani();

                if (tmpStr.Length != 0)
                {
                    IOHelper.SaveFile(_outputDirectory + file.Name, tmpStr);
                    //Console.WriteLine("File Processed :"+ file.Name);
                }
                else
                {
                    //Console.WriteLine("File Discarded :" + file.Name);
                }
                ConsoleUtil.DrawTextProgressBar(i, TextFiles.Length);
                i++;
                tmpStr = null;
            }
            Console.WriteLine("All Files Normalized.........");
        }
    }
}
