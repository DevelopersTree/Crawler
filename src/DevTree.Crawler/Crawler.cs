using Abot.Crawler;
using Abot.Poco;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevTree.BeKurdi;

namespace DevTree.Crawler
{
    public class Crawler
    {
        private const char IgnoreCharacter = '\0';

        private string _savePath = null;
        private int _delay;
        private int _maxPages;
        private HashSet<WebPage> _webPages;
        private IList<string> _TextContents;
        private string _statsFilePath;
        private const string StatsFileName = "$Stats.txt";
        private Uri _seedUrl;
        private static string _savedFilesDirectory;
        private static string _outputDirectory;
        public Crawler(string[] args)
        {
            
            _savePath = ParameterHelper.GetParameter(args, "-output", $" output path");
            _delay = ParameterHelper.GetIntegerParameter(args, "-delay", 1000);
            _maxPages = ParameterHelper.GetIntegerParameter(args, "-pages", 250);
            _statsFilePath = ParameterHelper.GetPath(_savePath, StatsFileName);
            _seedUrl = new Uri(ParameterHelper.GetParameter(args, "-url", " seed page"));
            
        }

        public HashSet<WebPage> Crawl()
        {
            _TextContents = new List<string>();
            
            if (File.Exists(_statsFilePath))
                File.Delete(_statsFilePath);

            var equalityComparer = ProjectionEqualityComparer.Create<WebPage, string>(page => page.Url);
            _webPages = new HashSet<WebPage>(equalityComparer);

            var crawler = GetDefaultWebCrawler(_maxPages, _delay);
            crawler.PageCrawlCompletedAsync += crawler_ProcessPageCrawlCompleted;

            CrawlResult result = crawler.Crawl(_seedUrl);
            return _webPages;
        }

        private IWebCrawler GetDefaultWebCrawler(int maxPagesToCrawl, int delayInMilliseconds)
        {
            var config = new CrawlConfiguration();
            config.CrawlTimeoutSeconds = 0;
            config.DownloadableContentTypes = "text/html, text/plain";
            config.IsExternalPageCrawlingEnabled = false;
            config.IsExternalPageLinksCrawlingEnabled = false;
            config.IsRespectRobotsDotTextEnabled = true;
            config.IsUriRecrawlingEnabled = false;
            config.IsHttpRequestAutoRedirectsEnabled = true;
            config.UserAgentString = "DevTree Crawler";
            config.MaxConcurrentThreads = 10;
            config.MaxPagesToCrawl = maxPagesToCrawl;
            config.MinCrawlDelayPerDomainMilliSeconds = delayInMilliseconds;

            return new PoliteWebCrawler(config);
        }
        private static void drawTextProgressBar(int progress, int total)
        {
            //draw empty progress bar
            Console.CursorLeft = 0;
            Console.Write("["); //start
            Console.CursorLeft = 32;
            Console.Write("]"); //end
            Console.CursorLeft = 1;
            float onechunk = 30.0f / total;

            //draw filled part
            int position = 1;
            for (int i = 0; i < onechunk * progress; i++)
            {
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.CursorLeft = position++;
                Console.Write(" ");
            }

            //draw unfilled part
            for (int i = position; i <= 31; i++)
            {
                Console.BackgroundColor = ConsoleColor.Green;
                Console.CursorLeft = position++;
                Console.Write(" ");
            }

            //draw totals
            Console.CursorLeft = 35;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write(progress.ToString() + " of " + total.ToString() + "    "); //blanks at the end remove any excess
        }
        void crawler_ProcessPageCrawlCompleted(object sender, PageCrawlCompletedArgs e)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var page = new WebPage
            {
                FileName = ParameterHelper.GetPath(_savePath, (_webPages.Count + 1).ToString() + ".txt"),
                Url = e.CrawledPage.Uri.AbsoluteUri
            };

            var success = _webPages.Add(page);
            
            if (!success)
            {
                Console.WriteLine("This page ha already been crawled...");
                return;
            }

            var contents = Extractor.Extract(e.CrawledPage.Content.Text);
            _TextContents.Add(contents);

            page.NumberOfWords = contents.Split(' ').Length;

            IOHelper.SaveFile(page.FileName, contents);

            SaveStats(page);

            stopWatch.Stop();
            Console.WriteLine("Pages crowled: " + _webPages.Count);
            Console.WriteLine($"Page Crawled: {page.Url}, Number Of Words: {page.NumberOfWords:n0}. Processed in {stopWatch.ElapsedMilliseconds:n0} ms");
        }
        public static  void ConvertToStandardText(string[] args)
        {
            _savedFilesDirectory = ParameterHelper.GetParameter(args, "-inputdir", " Input Directory ");
            _outputDirectory = ParameterHelper.GetParameter(args, "-outdir", " Output Directory ");
            
            DirectoryInfo dinfo = new DirectoryInfo(_savedFilesDirectory);
            FileInfo[] TextFiles = dinfo.GetFiles("*.txt");
            drawTextProgressBar(0, TextFiles.Length);
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
                drawTextProgressBar(i, TextFiles.Length);
                i++;
                tmpStr = null;
            }
            Console.WriteLine("All Files Normalized.........");
        } 



        public void SaveStats(WebPage page)
        {
            var content = $"{page.Url},{page.FileName},{page.NumberOfWords}{Environment.NewLine}";

            if (File.Exists(_statsFilePath))
                File.AppendAllText(_statsFilePath, content);
            else
                IOHelper.SaveFile(_statsFilePath, content);
        }

    }
}
