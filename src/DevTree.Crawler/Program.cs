using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTree.Crawler
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length >= 1)
            {
                switch (args[0].ToLower())
                {
                    case "crawl":
                        var pageCrawler = new Crawler(args);
                        var webPages = pageCrawler.Crawl();
                        break;

                    default:
                        WriteHelp();
                        break;
                }
            }
            else
            {
                WriteHelp();
            }

        }

        private static void WriteHelp()
        {
            Console.WriteLine(@"
Developers Tree Crawler
A little app used to manage the words list

For information about how to use this app, please go to: https://github.com/DevelopersTree/Crawler
");
        }
    }
}
