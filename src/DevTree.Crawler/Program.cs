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
            log4net.Config.XmlConfigurator.Configure();

            if (args.Length >= 1)
            {
                switch (args[0].ToLower())
                {
                    case "crawl":
                        var seedPagesFile = ParameterHelper.GetParameter(args, "-seed", "seed pages file");
                        var seedPages = File.ReadAllLines(seedPagesFile);
                        var savePath = ParameterHelper.GetParameter(args, "-path", $" save path");
                        var webPages = new List<WebPage>();
                        foreach (var page in seedPages)
                        {
                            var pageCrawler = new Crawler(args.Union(new string[] { "-url", page }).ToArray());
                            pageCrawler.Crawl(webPages);
                            pageCrawler.SaveStats();
                        }
                        
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

Examples:

crawler crawl -seed <link to txt file containing the urls> -path <path of folder to save the pages> [-delay <number of milliseconds : default is 1000>] [-maxpages <number of pages : default is 250>]

Note:
The entries in the text file for bulk-crawl should look like this:

https://google.com
https://en.wikipedia.org/wiki/Portal:History
");
        }
    }
}
