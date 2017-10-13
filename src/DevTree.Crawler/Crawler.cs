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

        private static string _savePath;
        private static string _statsFilePath;
        private static HashSet<WebPage> _webPages;
        private const string StatsFileName = "$Stats.txt";

        public static HashSet<WebPage> Crawl(string[] args)
        {
            _savePath = ParameterHelper.GetParameter(args, "-output", $" output path");
            _statsFilePath = ParameterHelper.GetPath(_savePath, StatsFileName);

            var delay = ParameterHelper.GetIntegerParameter(args, "-delay", 1000);
            var maxPages = ParameterHelper.GetIntegerParameter(args, "-pages", 250);
           
            var seedUrl = new Uri(ParameterHelper.GetParameter(args, "-url", " seed page"));

            if (File.Exists(_statsFilePath))
                File.Delete(_statsFilePath);

            var equalityComparer = ProjectionEqualityComparer.Create<WebPage, string>(page => page.Url);
            _webPages = new HashSet<WebPage>(equalityComparer);

            var crawler = GetDefaultWebCrawler(maxPages, delay);
            crawler.PageCrawlCompletedAsync += crawler_ProcessPageCrawlCompleted;

            CrawlResult result = crawler.Crawl(seedUrl);
            return _webPages;
        }

        private static IWebCrawler GetDefaultWebCrawler(int maxPagesToCrawl, int delayInMilliseconds)
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
        
        private static void crawler_ProcessPageCrawlCompleted(object sender, PageCrawlCompletedArgs e)
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

            page.NumberOfWords = contents.Split(' ').Length;

            IOHelper.SaveFile(page.FileName, contents);

            SaveStats(page);

            stopWatch.Stop();
            Console.WriteLine("Pages crowled: " + _webPages.Count);
            Console.WriteLine($"Page Crawled: {page.Url}, Number Of Words: {page.NumberOfWords:n0}. Processed in {stopWatch.ElapsedMilliseconds:n0} ms");
        }

        public static void SaveStats(WebPage page)
        {
            var content = $"{page.Url},{page.FileName},{page.NumberOfWords}{Environment.NewLine}";

            if (File.Exists(_statsFilePath))
                File.AppendAllText(_statsFilePath, content);
            else
                IOHelper.SaveFile(_statsFilePath, content);
        }

    }
}
